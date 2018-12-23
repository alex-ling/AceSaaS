using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Util;

namespace Acesoft.Core
{
    public class IdWorker : IIdWorker
    {
        //基准时间
        public const long Twepoch = 1483200000657L;

        //机器标识位数，0-31
        private const int WorkerIdBits = 5;
        //IDC标志位数，0-31
        private const int DatacenterIdBits = 5;
        //序列号识位数，QPS=409.6w/s
        private const int SequenceBits = 12;
        //机器标识最大值
        const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        //IDC标志最大值
        const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        //序列号ID最大值
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);
        //机器偏左移12位
        private const int WorkerIdShift = SequenceBits;
        //IDC偏左移17位
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        //时间毫秒左移22位
        public const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

        private long lastTimestamp = -1L;
        private readonly object @lock = new Object();

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }
        public long Sequence { get; internal set; }

        public IdWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new AceException($"WorkerId必须大于0，不能大于{MaxWorkerId}");
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new AceException($"DatacenterId必须大于0，不能大于{MaxDatacenterId}");
            }

            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;
        }

        public virtual string NextStringId()
        {
            return NaryHelper.FromNary(NextId(), 36);
        }

        public virtual long NextId()
        {
            lock (@lock)
            {
                var timestamp = TimeGen();
                if (timestamp < lastTimestamp)
                {
                    throw new AceException($"时间回退，拒绝为{lastTimestamp - timestamp}毫秒生成Id");
                }

                // 如果上次生成时间和当前时间相同,在同一毫秒内
                if (lastTimestamp == timestamp)
                {
                    // sequence自增，和sequenceMask相与一下，去掉高位
                    Sequence = (Sequence + 1) & SequenceMask;
                    // 判断是否溢出,也就是每毫秒内超过1024，当为1024时，与sequenceMask相与，sequence就等于0
                    if (Sequence == 0)
                    {
                        // 等待到下一毫秒
                        timestamp = TilNextMillis(lastTimestamp);
                    }
                }
                else
                {
                    // 如果和上次生成时间不同,重置sequence，就是下一毫秒开始，sequence计数重新从0开始累加,
                    // 为了保证尾数随机性更大一些,最后一位可以设置一个随机数
                    Sequence = 0;//new Random().Next(10);
                }

                lastTimestamp = timestamp;
                return ((timestamp - Twepoch) << TimestampLeftShift) | (DatacenterId << DatacenterIdShift) | (WorkerId << WorkerIdShift) | Sequence;
            }
        }

        // 防止产生的时间比之前的时间还要小（由于NTP回拨等问题）,保持增量的趋势.
        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        // 获取当前的时间戳
        protected virtual long TimeGen()
        {
            return DatetimeHelper.GetNowMilliseconds();
        }
    }
}
