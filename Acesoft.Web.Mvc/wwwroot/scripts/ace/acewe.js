// ACE libray
(function ($) {
    var ax = {
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
                debug: opts.debug, 
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
        wxError: function (xhr) {
            try {
                var msg = xhr.responseJSON.error_description || xhr.responseJSON.error;
                ax.wxFail(msg);
            } catch (ex) {
                ax.wxWarn(xhr.status + '：' + xhr.statusText);
            }
        },
        wxScan: function (cb) {
            wx.scanQRCode({
                needResult: 1,
                scanType: ["qrCode"/*, "barCode"*/],
                success: function (res) {
                    cb(res.resultStr);
                }
            });
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

    // merge to AX
    $.extend(AX, ax);
})(jQuery);