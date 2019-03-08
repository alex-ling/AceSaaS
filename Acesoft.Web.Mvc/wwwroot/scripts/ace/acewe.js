// ACE libray
(function (w) {
    var ax = {
        // options
        opts: {
            w: 300,
            h: 450,
            url: w.location.href,
            maxW: $(w.top).width(),
            maxH: $(w.top).height(),
            dialogMode: true,
            _modified: false,
            _curMods: []
        },

        // functions.
        init: function (opts) {
            opts = opts || {};
            this.opts = $.extend(this.opts, opts);
            ax.root = opts.root || '/';
        },
        modify: function () {
            ax.opts._modified = true;
        },
        go: function (url) {
            w.location = ax.aurl(url, "t", ax.tick());
        },
        goBack: function () {
            window.location = "native://goback";
        },
        refresh: function () {
            w.location = ax.aurl(w.location.href, "t", ax.tick());
        },
        query: function (name, url) {
            if (typeof url == 'undefined') {
                url = w.location.search;
            }
            if (typeof name != 'undefined') {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
                var r = url.substr(1).match(reg);
                if (r != null) return unescape(r[2]);
                return null;
            }
            else {
                var vars = [], hash;
                var hashes = url.substr(1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            }
        },
        aurl: function (url, name, val) {
            var i = url.indexOf(name + '=');
            if (i > 0) {
                var e = url.indexOf('&', i);
                if (e > i) return url.replace(url.substr(i, e - i), name + '=' + val);
                else return url.substr(0, i) + name + '=' + val;
            } else {
                return url.indexOf('?') > 0 ? (url + '&' + name + '=' + val) : (url + '?' + name + '=' + val);
            }
        },
        api: function (opts) {
            var url = ax.format("{0}api/{1}", ax.opts.root, opts.api);
            if (opts.ds) {
                url = ax.aurl(url, "app", ax.opts.app);
                url = ax.aurl(url, "ds", opts.ds);
            }
            if (opts.q) {
                url += url.indexOf('?') > 0 ? '&' : '?';
                url += opts.q;
            }
            return url;
        },
        ajaxerror: function (xhr, status, error) {
            try {
                var msg = xhr.responseJSON.error_description || xhr.responseJSON.error;
                ax.wxFail(msg);
            } catch (ex) {
                ax.wxWarn(xhr.status + '：' + xhr.statusText);
            }
        },
        ajax: function (opts) {
            opts.success = opts.success || function (rv, status, xhr) {
                if (opts.cb) {
                    opts.cb(rv);
                }
            };
            if (opts.type == 'post' || opts.type == 'put') {
                opts.data = JSON.stringify(opts.data);
                opts.dataType = opts.dataType || 'json';
                opts.contentType = opts.contentType || 'application/json';
            }
            opts.error = opts.error || ax.ajaxerror;
            $.ajax(opts);
        },

        // util
        len: function (s) {
            var l = 0;
            for (var i = 0; i < s.length; i++ , l++) {
                if (s[i].match(/[^x00-xff]/ig) != null) l++;
            };
            return l;
        },
        now: function () {
            return new Date();
        },
        tick: function () {
            return Date.parse(new Date()) / 1000;
        },
        pad: function (s, ch, len, left) {
            if (s.length < len) {
                var ns = '';
                for (i = 1; i <= (len - s.length); i++) {
                    ns += ch;
                }
                return left ? (ns + s) : (s + ns);
            }
            else {
                return s;
            }
        },
        format: function () {
            var args = arguments;
            if (args.length < 1) return null;
            return args[0].replace(/\{(\d+)(:(0{2,}))?\}/g, function (m, i) {
                var ix = m.indexOf(':'), val = $.isArray(args[1]) ? args[1][i].toString() : args[++i].toString();
                if (ix < 0) return val;
                var fmt = m.substring(ix + 1, m.length - 1);
                if (fmt.length < val.length) return val;
                return fmt.substr(0, fmt.length - val.length) + val;
            });
        },
        aurl: function (url, name, val) {
            var i = url.indexOf(name + '=');
            if (i > 0) {
                var e = url.indexOf('&', i);
                if (e > i) return url.replace(url.substr(i, e - i), name + '=' + val);
                else return url.substr(0, i) + name + '=' + val;
            } else {
                return url.indexOf('?') > 0 ? (url + '&' + name + '=' + val) : (url + '?' + name + '=' + val);
            }
        },
        objstr: function (s, o) {
            if (!o) return s;
            return s.replace(/\{([^\d\{\}]+)([\w\_]+)?\}/g, function (m, i) {
                return o[i];
            });
        },
        rndstr: function (len) {
            len = len || 32;
            var chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678';
            var maxPos = chars.length;
            var rv = '';
            for (var i = 0; i < len; i++) {
                rv += chars.charAt(Math.floor(Math.random() * maxPos));
            }
            return rv;
        },
        isdate: function (s) {
            var d = ax.dateobj(s);
            return !isNaN(d) || !isNaN(d2);
        },
        date: function (mask) {
            return ax.datestr(ax.now, mask);
        },
        dateobj: function (s) {
            var ss = s.match(/\d+/g);
            return new Date(ss[0], ss[1] - 1, ss[2], ss[3] || 0, ss[4] || 0, ss[5] || 0);
        },
        datestr: function (d, mask) {
            if (typeof d == 'string') d = ax.dateobj(d);
            return mask.replace(/"[^"]*"|'[^']*'|\b(?:d{1,4}|M{1,4}|yy(?:yy)?|([hHmstT])\1?|[lLZ])\b/g, function ($0) {
                switch ($0) {
                    case 'd': return d.getDate();
                    case 'dd': return ax.format('{0:00}', d.getDate());
                    case 'ddd': return $.fn.calendar.defaults.weeks[d.getDay()];
                    case 'dddd': return '星期' + $.fn.calendar.defaults.weeks[d.getDay()];
                    case 'M': return d.getMonth() + 1;
                    case 'MM': return ax.format('{0:00}', d.getMonth() + 1);
                    case 'MMM': return $.fn.calendar.defaults.months[d.getMonth()].replace('月', '');
                    case 'MMMM': return $.fn.calendar.defaults.months[d.getMonth()];
                    case 'yy': return String(d.getFullYear()).substr(2);
                    case 'yyyy': return d.getFullYear();
                    case 'h': return d.getHours() % 12 || 12;
                    case 'hh': return ax.format('{0:00}', d.getHours() % 12 || 12);
                    case 'H': return d.getHours();
                    case 'HH': return ax.format('{0:00}', d.getHours());
                    case 'm': return d.getMinutes();
                    case 'mm': return ax.format('{0:00}', d.getMinutes());
                    case 's': return d.getSeconds();
                    case 'ss': return ax.format('{0:00}', d.getSeconds());
                    case 'l': return ax.format('{0:000}', d.getMilliseconds(), 3);
                    case 'L': var m = d.getMilliseconds(); if (m > 99) m = Math.round(m / 10); return ax.format('{0:00}', m);
                    case 'tt': return d.getHours() < 12 ? 'am' : 'pm';
                    case 'TT': return d.getHours() < 12 ? 'AM' : 'PM';
                    case 'Z': return d.toUTCString().match(/[A-Z]+$/);
                    default: return $0.substr(1, $0.length - 2);
                }
            });
        },

        // wechat
        wxRun: function () {
            var ua = navigator.userAgent.toLowerCase();
            return ua.indexOf('micromessenger') != -1;
        },
        wxUrl: function () {
            var url = ax.aurl(w.location.href, 't', ax.tick());
            w.history.replaceState(null, null, url);
        },
        wxInit: function (opts) {
            wx.config({
                beta: true,
                debug: false, 
                appId: opts.appId,
                timestamp: opts.timestamp,
                nonceStr: opts.nonce,
                signature: opts.signature,
                jsApiList: opts.jsApi
            });
            if (opts.resetUrl) {
                var url = ax.aurl(w.location.href, 't', opts.timestamp);
                history.replaceState(null, null, url);
            }
        },
        wxOk: function (text) {
            $.toptip(text || '操作成功', 2000, 'success');
        },
        wxFail: function (text) {
            $.toptip(text || '操作失败', 2000, 'error');
        },
        wxWarn: function (text) {
            $.toptip(text, 2000, 'warning');
        },
        wxScan: function (cb) {
            wx.scanQRCode({
                needResult: 1,
                scanType: ["qrCode"/*, "barCode"*/],
                success: function (res) {
                    cb(res.resultStr);
                }
            })
        },
        wxWiFi: function (cb) {
            wx.invoke('configWXDeviceWiFi', {}, function (res) {
                if (res.err_msg == 'configWXDeviceWiFi:ok') {
                    cb();
                }
                else if (res.err_msg == 'configWXDeviceWiFi:cancel') {
                    ax.wxWarn('配网已取消！');
                }
                else {
                    ax.wxFail('配网失败，请确认已按配网要求进行操作！');
                }
            });
        },
        wxMap: function (lat, lon, name, addr, url) {
            wx.openLocation({
                latitude: lat,
                longitude: lon,
                name: name,
                address: addr,
                scale: 14,
                infoUrl: url || null
            });
        },
        wxLoc: function (cb, fail) {
            wx.getLocation({
                type: 'wgs84',
                success: function (res) {
                    cb(res.longitude, res.latitude);
                },
                cancel: fail || function (res) {
                    ax.wxWarn('获取定位失败，用户拒绝授权！');
                }
            });
        },
        wxPage: function () {
            var loading = false, attach = true, $pg = $('.page'), $bd= $('.page .bd'),
                h_load = '<i class="weui-loading"></i><span class="weui-loadmore__tips">正在加载</span>',
                h_empty = '<span class="weui-loadmore__tips">已无更多数据啦</span>',
                p = new Function("return {" + $('pager').attr('data-options') + "}")(),
                init = function () { $pg.infinite().find('.weui-loadmore').html(h_load) },
                destory = function () { $pg.destroyInfinite().find('.weui-loadmore').html(h_empty) };
            p.pageNumber == p.pageCount ? destory() : init();
            $pg.pullToRefresh().on('pull-to-refresh', function (e) {
                $.get(w.location.href, function (res) {
                    $('.weui-photo-browser-modal').remove();
                    $('.weui-cells').replaceWith($(res).find('.weui-cells'));
                    $pg.pullToRefreshDone();
                    if (!attach) {
                        init();
                    }
                });
            });
            $pg.on('infinite', function () {
                if (loading) return;
                loading = true;
                p = new Function("return {" + $('pager').attr('data-options') + "}")();
                if (p.pageNumber < p.pageCount) {
                    var url = AX.aurl(w.location.href, 'page', p.pageNumber + 1);
                    $.get(url, function (res) {
                        $('pager').replaceWith($(res).find('.weui-cells').children());
                        if (p.pageNumber + 1 == p.pageCount) {
                            attach = false;
                            destory();
                        }
                        loading = false;
                    });
                }
            });
        },
        wxPhoto: function (index) {
            var $ele = $(event.srcElement).closest('.weui-cell');
            if (!$ele.length) $ele = $(event.srcElement).parent();
            var pb = $ele.data('photoBrowser');
            if (!pb) {
                pb = $.photoBrowser({
                    items: $ele.attr('data-photos').split(',')
                });
                $ele.data('photoBrowser', pb);
            }
            pb.open(index);
        },

        setUser: function (data, cb) {
            ax.ajax({
                url: ax.api({ api: 'auth/putuser' }),
                data: data,
                type: 'put',
                cb: function () {
                    ax.wxOk('修改成功！');
                    cb();
                }
            });
        },
        setUid: function (cur, cb) {
            if (cur != '') return;
            $.prompt({
                title: '设置登录名',
                text: '登录名由数字和字母组成，20个字符以内，设置成功后不能再次修改！',
                input: '登录名',
                empty: false,
                onOK: function (val) {
                    if (val.length < 20) {
                        ax.setUser({ loginname: val }, function () {
                            cb(val);
                        });
                    }
                    else {
                        ax.wxWarn('登录名不能超过20个字符！');
                    }
                }
            });
        },
        setNik: function (nick, cb) {
            $.prompt({
                title: '设置昵称',
                text: '昵称不能超过10个字符，可以再改！',
                input: nick,
                empty: false,
                onOK: function (val) {
                    if (val.length < 20) {
                        ax.setUser({ nickname: val }, function () {
                            cb(val);
                        });
                    }
                    else {
                        ax.wxWarn('昵称不能超过20个字符！');
                    }
                }
            });
        }
    };

    // AX
    w.AX = ax;
})(window);