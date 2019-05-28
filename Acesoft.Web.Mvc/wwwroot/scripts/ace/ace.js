// ACE libray
(function (w) {
    var ax = {
        // options
        opts: {
            w: 750,
            h: 450,
            url: w.location.href,
            maxW: $(w.top).width(),
            maxH: $(w.top).height(),
            dialogMode: true,
            _modified: false,
            _curTreeId: null,
            _uploadedFiles: [],
            _curMods: []
        },

        // functions.
        init: function (opts) {
            opts = opts || {};
            this.opts = $.extend(this.opts, opts);
            AX.root = opts.root || '/';
        },
        modify: function () {
            ax.opts._modified = true;
        },
        wxRun: function () {
            var ua = navigator.userAgent.toLowerCase();
            return ua.indexOf('micromessenger') != -1;
        },
        query: function (name) {
            if (typeof name != 'undefined') {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
                var r = w.location.search.substr(1).match(reg);
                if (r != null) return unescape(r[2]);
                return null;
            }
            else {
                var vars = [], hash;
                var hashes = w.location.search.substr(1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            }
        },
        close: function () {
            w.open('', '_self', '');
            w.close();
        },
        refresh: function () {
            w.location = w.location;
        },
        index: function () {
            if (ax.opts.app == "app") {
                w.location = ax.opts.root + "app";
            }
            else {
                w.location = ax.opts.root;
            }
        },
        login: function () {
            w.location = ax.format('{0}?app={1}', ax.opts.loginUrl, ax.opts.app);
        },
        enter: function () {
            w.location = ax.format('{0}plat/account/enter?app={1}', ax.opts.root, ax.opts.app);
        },
        logout: function () {
            w.location = ax.format('{0}?app={1}', ax.opts.logoutUrl, ax.opts.app);
        },
        tfocus: function () {
            var txts = $('input:text,input:password').not(':hidden');
            //txts.first().focus();
            txts.bind('keydown', function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    var next = txts.index(this) + 1;
                    if (next < txts.length) txts[next].focus();
                    else login();
                }
            });
        },

        // go
        path: function (url) {
            return AX.opts.path + url;
        },
        app: function (url) {
            return ax.format("{0}{1}/{2}", ax.opts.root, ax.opts.app, url);
        },

        // 加载页面
        load: function (url) {
            if (url.substr(0, 1) != '/') url = ax.path(url);
            $('.ct-wrap').find('iframe').attr('src', url);
        },
        go: function (url) {
            var ix = url.indexOf('|');
            var acts, mod = ix > 0 ? url.substr(0, ix) : url;
            $("[data-url='" + mod + "']").trigger('click');
            url = url.substr(ix + 1);
            acts = url.split('/');
            $("[data-url='" + mod + "/" + acts[0] + "']").trigger('click');
            if (acts.length > 1) {
                AX.load("/" + mod + "/" + url);
            }
        },
        gourl: function (url) {
            w.location = url.substr(0, 1) == '/' ? url : AX.app(url);
        },
        initFt: function () {
            $('.ct-title').append('<button class="fr btn btn-default btn-xs ml20 mt5" onclick="AX.refresh()"><span class="fa fa-refresh"></span> 刷新</button>');
            $('.ct-wrap .btn[data-cmd]').click(function () {
                var cmd = $(this).attr('data-cmd');
                if (cmd == 'reload') AX.refresh();
                else if (cmd == 'save') onSubmit(AX.ok);
                else if (cmd == 'save-back') {
                    onSubmit(function () {
                        AX.ok();
                        AX.gourl($(this).attr('data-url'));
                    });
                }
            });
        },
        initBk: function () {
            $('.ct-title').append('<button class="fr btn btn-default btn-xs ml10 mt8" data-cmd="max"><span class="fa fa-window-maximize"></span> 全屏打开</button>');
            $('.ct-title').append('<button class="fr btn btn-default btn-xs ml20 mt8" data-cmd="reload"><span class="fa fa-refresh"></span> 刷新</button>');
            $('.ct-wrap .btn[data-cmd]').click(function () {
                var cmd = $(this).attr('data-cmd');
                if (cmd == 'reload') AX.reload();
                else if (cmd == 'max') window.open(window.location.href.replace('&nav=1', ''));
                else if (cmd == 'back') AX.back();
                else if (cmd == 'save') onSubmit(AX.ok);
                else if (cmd == 'save-back') {
                    onSubmit(function () {
                        AX.ok();
                        if (cmd.indexOf('back') >= 0) AX.back();
                    });
                }
            });
        },
        // 返回上一级
        back: function () {
            w.history.back();
        },
        // 刷新
        reload: function () {
            AX.refresh();
        },

        // form.
        onLogin: function () {
            AX.formSubmit('#log', function (url) {
                $.messager.info({ msg: '登录成功' });
                window.location = url;
            });
        },
        formSubmit: function (fid, cb, q, x) {
            var jq = $(fid);
            if (jq.length == 0) return;
            if (typeof cb == 'undefined') cb = null;
            $('.aceui-kindeditor').kindeditor('sync');
            if (jq.form('validate')) {
                var cbSave = function () {
                    var f = jq.data('form');
                    var url = f.options.url;
                    var d = jq.serializeObject();
                    var dd = f.options.isEdit ? $.extend(d, { id: f.data.id }) : $.extend(f.data, d);
                    if (q) url += "&" + q;
                    if (x) dd = $.extend(dd, x);
                    ax.ajax($.extend({}, f.options, {
                        url: url,
                        cb: cb,
                        data: dd,
                        headers: {
                            RequestVerificationToken: dd.__RequestVerificationToken
                        }
                    }));
                };
                for (var i = 0; i < AX.opts._uploadedFiles.length; i++) {
                    var swfu = AX.opts._uploadedFiles[i];
                    if (swfu.settings.required) {
                        if (swfu.settings.target.val() == "" && swfu.getStats().files_queued == 0) {
                            $.messager.error({ msg: "请选择要上传的文件！" });
                            return false;
                        }
                    }
                    if (swfu.getStats().files_queued > 0) {
                        swfu.settings.onComplete = cbSave;
                        swfu.startUpload();
                        return false;
                    }
                }
                cbSave();
            }
        },
        formReset: function (fid) {
            var jq = $(fid);
            if (jq.length == 0) return;
            jq.form('reset');
        },
        formLoad: function (fid, data) {
            $(fid).data('form').data = data;
            $(fid).form('load', data);
        },
        // combo.
        selShow: function () {
            var c = $(this), opts = c.combo('options');
            var val = opts.multiple ? (c.combo('getValues') + '&multi=1') : c.combo('getValue');
            ax.dialog('请选择', ax.aurl(opts.url, 'val', val), function (val, txt) {
                if (!opts.onBeforeChange || opts.onBeforeChange.call(c, val, txt) != false) {
                    c.combo('setValue', val);
                    c.combo('setText', txt);
                } else {
                    return true;
                }
            }, opts.panelWidth, opts.panelHeight);
            return false;
        },
        btnShow: function (obj) {
            var c = $(obj), opts = c.linkbutton('options');
            var vBox = $('#' + opts.valueBox), tBox = $('#' + opts.textBox);
            var val = vBox.textbox('getValue'), txt = tBox.textbox('getValue');
            ax.dialog('请选择', ax.aurl(opts.url, 'val', val), function (val, txt) {
                if (!opts.onBeforeChange || opts.onBeforeChange.call(c, val, txt) != false) {
                    vBox.textbox('setValue', val);
                    tBox.textbox('setValue', txt);
                    if (opts.onChange) opts.onChange.call(c, val, txt);
                } else {
                    return true;
                }
            }, opts.dialogWidth || ax.opts.w, opts.dialogHeight || ax.opts.h);
            return false;
        },
        selText: function (v) {
            $(this).combo('setText', v);
        },
        // tree.
        treeAdd: function (tid, level, url) {
            var opts = { level: level };
            if (url) opts.editUrl = url;
            $(tid).tree('add', opts);
        },
        treeEdit: function (tid, level, url) {
            var opts = { level: level };
            if (url) opts.editUrl = url;
            $(tid).tree('edit', opts);
        },
        treeDel: function (tid, level, ds) {
            var opts = { level: level, ds: ds };
            $(tid).tree('del', opts);
        },
        // grid.
        gridSrh: function (gid, fid) {
            var param, jq = $(gid), form = $(fid);
            if (jq.hasClass("easyui-datagrid")) {
                param = $.extend(jq.datagrid('options').queryParams, form.serializeObject());
                jq.datagrid('load', param);
            } else {
                param = $.extend(jq.treegrid('options').queryParams, form.serializeObject());
                jq.treegrid('load', param);
            }
        },
        gridClr: function (gid, fid) {
            ax.formReset(fid);
            ax.gridSrh(gid, fid);
        },
        gridEx: function (gid) {
            var jq = $(gid);
            var opts = jq.hasClass("easyui-datagrid") ? jq.datagrid('options') : jq.datagrid('options');
            var q = opts.url.split('?').length > 1 ? opts.url.split('?')[1] : '';
            var qp = $.param(opts.queryParams);
            if (qp) q += '&' + $.param(opts.queryParams);
            var url = ax.api({ api: 'excel/down', q: q + '&path=' + ax.opts.path });
            var form = ax.format('<form action="{0}" method="post"></form>', AX.aurl(url, 'rows', '0'));
            $(form).appendTo('body').submit().remove();
        },
        gridTb: function (gid) {
            var jq = typeof gid == 'string' ? $(gid) : gid;
            var tb = [{
                iconCls: 'icon-excel',
                text: '&nbsp;导出Excel文件',
                handler: function () { AX.gridEx(gid); }
            }];
            if (jq.hasClass("easyui-datagrid")) {
                jq.datagrid('getPager').pagination({ buttons: tb });
            } else {
                jq.treegrid('getPager').pagination({ buttons: tb });
            }
        },
        gridAdd: function (gid, query) {
            if ($(gid).hasClass("easyui-datagrid")) $(gid).datagrid('add', query);
            else $(gid).treegrid('add', query);
        },
        gridEdit: function (gid, id) {
            if ($(gid).hasClass("easyui-datagrid")) $(gid).datagrid('edit', id);
            else $(gid).treegrid('edit', id);
        },
        gridDel: function (gid, id) {
            if ($(gid).hasClass("easyui-datagrid")) $(gid).datagrid('del', id);
            else $(gid).treegrid('del', id);
        },
        gridAct: function (gid, act, id) {
            if (act == 'del') ax.gridDel(gid, id);
            else if (act == 'edit') ax.gridEdit(gid, id);
            else new Function(ax.format('event_{0}_{1}("{2}","{3}");', gid.substr(1), act, gid, id))();
        },
        gridFmt: function (v, rd, ri) {
            if (v == null || v === '') return v;
            var fmt = this.format.split(':')[0];
            var exp = this.format.substr(this.format.indexOf(':') + 1);
            switch (fmt) {
                case 'date':
                    if (ax.isdate(v)) return ax.datestr(v, exp);
                case 'bool':
                    return v == "1" ? '<span class="icon icon-ok"></span>' : '';
                case 'action':
                    return ax.format('<a href="javascript:void()" class="grid" onclick="{0}(\'' + v + '\',\'' + rd.id + '\')" title="{1}">', exp.split('=')) + v + '</a>';
                case 'link':
                    if (v.substr(0, 1) == ',') v = v.substr(1);
                    if (v != '') return '<a target="_blank" href="' + ax.objstr(exp, rd) + '">' + v + '</a>';
                    return v;
                case 'href':
                    if (v.substr(0, 1) == ',') v = v.substr(1);
                    return '<a target="_blank" href="' + v + '">' + exp + '</a>'
                case 'icon':
                    return '<span class="icon ' + v + '"></span>';
                case 'text':
                    return v.replace(/\r\n/g, "<br/>");
                case 'button':
                    var html = '', ops = v.split(',');
                    for (var i = 0; i < ops.length; i++) {
                        var op = ops[i].split('=')[0], title = ops[i].split('=')[1];
                        html += ax.format('<a class="easyui-linkbutton aceui" data-options="iconCls:\'fa fa-{0}\',text:\'{5}\'" onclick="AX.gridAct(\'{4}\',\'{1}\',\'{3}\')" title="{2}"></a>',
                            op.split('_').length > 1 ? op.split('_')[1] : op, op.split('_')[0], title, rd.id, exp, op.split('_').length > 2 ? op.split('_')[2] : '');
                    }
                    return html;
                case 'attach':
                    var items = v.split(','), html = '';
                    for (var i = 1; i < items.length; i++) {
                        html += ax.format('<div class="lh20"><a href="{0}{1}" target="_blank" title="{2}">{3}{2}</a></div>',
                            items[i].substr(0, 4) == 'http' ? '' : ax.opts.root, 
                            items[i], ax.filename(items[i]).substr(11), (items.length > 2 ? (i + '.') : ''));
                    }
                    return html;
                case 'qrcode':
                    return ax.format('<img src="/api/draw/getqrcode?text={0}" />', v);
                case 'tip':
                    return ax.format('<a href="javascript:;" class="easyui-tooltip" data-options="content:$(\'<div></div>\'),onUpdate:{1}">{0}</a>', v, exp);
                case 'size':
                    return ax.filesize(v);
                default:
                    return v;
            }
        },
        // dialog.
        dialog: function (title, url, cb, w, h, ccb) {
            if (typeof w == 'undefined') w = ax.opts.w;
            if (typeof h == 'undefined') h = ax.opts.h;
            if (url.substr(0, 1) != '/') url = ax.path(url);
            var btns = cb == null ? null : [{
                text: '确定', iconCls: 'fa fa-check', handler: function () {
                    $("iframe", win).get(0).contentWindow.onSubmit(function () {
                        if (!cb.apply(this, arguments)) d.dialog('destroy');
                    });
                }
            }, {
                text: '取消', iconCls: 'fa fa-close', handler: function () {
                    if (ccb) ccb.apply(this, arguments);
                    ax.dlgClose($("iframe", win).get(0).contentWindow, d);
                }
            }];
            var win = window.top.$('<div class="dialog"></div>');
            var d = win.dialog({
                width: w, height: h, modal: true, resizable: true, title: title, maximizable: true,
                onOpen: function () {
                    $("iframe", win).attr("src", url);
                },
                content: '<iframe frameborder="0" style="width:100%;height:100%;margin:0px;padding:0px"></iframe>',
                onBeforeClose: function () {
                    return ax.dlgClose($("iframe", win).get(0).contentWindow, d);
                },
                buttons: btns
            });
        },
        dclose: function () {
            ax.dlgClose(window, window.parent.$('.dialog').last());
        },
        dlgClose: function (w, d) {
            if (w._modified && confirm('数据已经修改，确定关闭窗口？')) d.dialog('destroy');
            else if (!w._modified) d.dialog('destroy');
            return false;
        },

        // util
        root: '/',
        ok: function (msg) {
            if (typeof msg == 'string') $.messager.info({ msg: msg });
            else $.messager.info({ msg: '操作成功！' });
        },
        error: function (msg) {
            if (typeof msg == 'string') $.messager.error({ msg: msg });
            else $.messager.error({ msg: '操作失败！' });
        },
        okcall: function (cb) {
            $.messager.alert('提示', '操作成功！', 'info', cb);
        },
        ajaxerror: function (xhr, status, error) {
            try {
                var msg = xhr.responseJSON.error_description || xhr.responseJSON.error;
                $.messager.error({ msg: msg });
            } catch (ex) {
                $.messager.error({ msg: xhr.status + '：' + xhr.statusText });
            }
        },
        ajax: function (opts) {
            opts.success = opts.success || function (rv) {
                if (opts.cb) opts.cb(rv);
            };
            if (opts.type == 'post' || opts.type == 'put') {
                opts.data = JSON.stringify(opts.data);
                opts.dataType = opts.dataType || 'json';
                opts.contentType = opts.contentType || 'application/json';
            }
            opts.error = opts.error || this.ajaxerror;
            $.ajax(opts);
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
        ext: function (file) {
            var pos = file.lastIndexOf('.');
            return pos > 0 ? file.substring(pos) : '';
        },
        filename: function (file) {
            var pos = file.lastIndexOf('/');
            return pos > 0 ? file.substring(pos + 1) : file;
        },
        filesize: function (len) {
            var rv = "";
            if (len < 1048576) rv = (len / 1024).toFixed(2) + "KB";
            else if (len == 1048576) rv = "1MB";
            else if (len > 1048576 && len < 1073741824) rv = (len / (1024 * 1024)).toFixed(2) + "MB";
            else if (len > 1048576 && len == 1073741824) rv = "1GB";
            else if (len > 1073741824 && len < 1099511627776) rv = (len / (1024 * 1024 * 1024)).toFixed(2) + "GB";
            else rv = ">1TB";
            return rv;
        },
        len: function (s) {
            var l = 0;
            for (var i = 0; i < s.length; i++ , l++) {
                if (s[i].match(/[^x00-xff]/ig) != null) l++;
            }
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
        tohex: function (n, len) {
            var num = typeof n == 'number' ? n : parseInt(n);
            return ax.pad(num.toString(16).toUpperCase(), '0', len, 1);
        },
        fromhex: function (s) {
            return parseInt(s, 16);
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
        iframe: {
            resolution: 200,
            iframes: [],
            interval: null,
            Iframe: function () {
                this.element = arguments[0];
                this.cb = arguments[1];
                this.hasTracked = false;
            },
            track: function (element, cb) {
                this.iframes.push(new this.Iframe(element, cb));
                if (!this.interval) {
                    var _this = this;
                    this.interval = setInterval(function () { _this.checkClick(); }, this.resolution);
                }
            },
            checkClick: function () {
                if (document.activeElement) {
                    var activeElement = document.activeElement;
                    for (var i in this.iframes) {
                        if (activeElement === this.iframes[i].element) {
                            if (this.iframes[i].hasTracked == false) {
                                this.iframes[i].cb.apply(window, []);
                                this.iframes[i].hasTracked = true;
                            }
                        } else {
                            this.iframes[i].hasTracked = false;
                        }
                    }
                }
            }
        }
    };

    // AX
    w.AX = ax;
})(window);

// $.ready
$(function () {
    // init aceui controls.
    if ($.aceui) {
        $.aceui.auto && $.aceui.parse();
    }
    // auto focus.
    AX.tfocus();
});