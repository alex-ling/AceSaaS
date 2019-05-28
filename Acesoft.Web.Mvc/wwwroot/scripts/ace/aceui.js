// $.aceui: parse aceui automatic.
(function ($) {
    // 定义aceui及selector[.aceui-widget]转化方法
    $.aceui = {
        auto: true,
        plugins: ['uploadbox', 'kindeditor', 'echart', 'iframe'],
        parse: function (context) {
            for (var i = 0; i < $.aceui.plugins.length; i++) {
                var name = $.aceui.plugins[i], ctor = name;
                if (typeof name != 'string') {
                    name = name[0];
                    ctor = ctor[1];
                }
                var r = $('.aceui-' + name, context);
                if (r.length) {
                    if (r[ctor]) {
                        r[ctor]();
                    }
                }
            }
        }
    };

    // 定义对象的扩展方法
    /* 得到日期年月日等加数字后的日期 */
    Date.prototype.dateAdd = function (interval, number) {
        var d = this;
        var k = { 'y': 'FullYear', 'q': 'Month', 'm': 'Month', 'w': 'Date', 'd': 'Date', 'h': 'Hours', 'n': 'Minutes', 's': 'Seconds', 'ms': 'MilliSeconds' };
        var n = { 'q': 3, 'w': 7 };
        eval('d.set' + k[interval] + '(d.get' + k[interval] + '()+' + ((n[interval] || 1) * number) + ')');
        return d;
    };
    /* 计算两日期相差的日期年月日等 */
    Date.prototype.dateDiff = function (interval, objDate2) {
        var d = this, i = {}, t = d.getTime(), t2 = objDate2.getTime();
        i['y'] = objDate2.getFullYear() - d.getFullYear();
        i['q'] = i['y'] * 4 + Math.floor(objDate2.getMonth() / 4) - Math.floor(d.getMonth() / 4);
        i['m'] = i['y'] * 12 + objDate2.getMonth() - d.getMonth();
        i['ms'] = objDate2.getTime() - d.getTime();
        i['w'] = Math.floor((t2 + 345600000) / (604800000)) - Math.floor((t + 345600000) / (604800000));
        i['d'] = Math.floor(t2 / 86400000) - Math.floor(t / 86400000);
        i['h'] = Math.floor(t2 / 3600000) - Math.floor(t / 3600000);
        i['n'] = Math.floor(t2 / 60000) - Math.floor(t / 60000);
        i['s'] = Math.floor(t2 / 1000) - Math.floor(t / 1000);
        return i[interval];
    };
})(jQuery);

// jQuery Exten.
(function ($) {
    /* rnd: return a random id.
     */
    $.rnd = function () {
        return ((1 + Math.random()) * 65536 | 0).toString(16).substring(1);
    };
    /* outerHtml: get or set the outer html.
     */
    $.fn.outerHtml = function (s) {
        return (s) ? this.before(s).remove() : $("<p>").append(this.eq(0).clone()).html();
    }
    /* serializeObject: serialize data to json.
     */
    $.fn.serializeObject = function () {
        "use strict";
        var data = this.serializeArray();
        $('.aceui-checkbox:not(:checked)').filter(function () {
            return this.name && !this.disabled;
        }).map(function () {
            return { name: this.name, value: 0 };
        }).each(function () {
            data.push(this);
        });
        var rv = {};
        $.each(data, function (i, ele) {
            var p = rv[ele.name];
            if (typeof (p) != 'undefined' && p != null) {               
                if (p && ele.value) rv[ele.name] = p + ',' + ele.value;
                else if (p) rv[ele.name] = p;
                else if (ele.value) rv[ele.name] = ele.value;
                else rv[ele.name] = null;
            } else {
                rv[ele.name] = ele.value;
            }
        });
        return rv;
    };
    /* center: center to inside element.
     */
    $.fn.center = function (opts) {
        var opts = $.extend({
            inside: null,       // default element's parent
            duration: 0,        // millisecond, transition time
            minX: 10,            // pixel, minimum left element value
            minY: 10,            // pixel, minimum top element value
            scrolling: false,   // booleen, take care of the scrollbar (scrollTop)
            v: true,            // booleen, center vertical
            h: true             // booleen, center horizontal
        }, opts);
        return this.each(function () {
            var props = { position: 'absolute' };
            var inside = opts.inside ? $(opts.inside) : $(this).parent();
            if (opts.v) {
                var top = (inside.height() - $(this).outerHeight()) / 2;
                if (opts.scrolling) top += inside.scrollTop() || 0;
                top = top > opts.minY ? top : opts.minY;
                top = inside.css("position") != "absolute" ? top : top + parseInt(inside.css("top"), 10);
                $.extend(props, { top: top + 'px' });
            }
            if (opts.h) {
                var left = (inside.width() - $(this).outerWidth()) / 2;
                if (opts.scrolling) left += inside.scrollLeft() || 0;
                left = left > opts.minX ? left : opts.minX;
                left = inside.css("position") != "absolute" ? left : left + parseInt(inside.css("left"), 10);
                $.extend(props, { left: left + 'px' });
            }
            if (opts.duration > 0) $(this).animate(props, opts.duration);
            else $(this).css(props);
            return $(this);
        });
    };
    /* acetip: extend tooltip for ant.
     */
    $.fn.acetip = function () {
        $(this).tooltip({
            position: 'right',
            content: function () {
                return '<span style="color:#fff;font:14px/1.5 \'微软雅黑\',YaHei;">' + $(this).attr('tooltip') + '</span>';
            },
            onShow: function () {
                $(this).tooltip('tip').css({
                    backgroundColor: '#425160',
                    borderColor: '#425160',
                    padding: '8px 10px',
                    opacity: '0.9',
                    filter: 'alpha(opacity = 90)',
                    'border-radius': '0'
                });
            }
        });
    };
    /* imgFloat: image float.
     */
    $.fn.imgFloat = function (options) {
        var own = this;
        var xD = 0;
        var yD = 0;
        var i = 1;
        var settings = {
            speed: 15,
            xPos: 0,
            yPos: 0
        };
        $.extend(settings, options);
        var ownTop = settings.xPos;
        var ownLeft = settings.yPos;
        own.css({
            position: "absolute",
            cursor: "pointer"
        });
        function imgPosition() {
            var winWidth = $(window).width() - own.width();
            var winHeight = $(window).height() - own.height();
            if (xD == 0) {
                ownLeft += i;
                own.css({
                    left: ownLeft
                });
                if (ownLeft >= winWidth) {
                    ownLeft = winWidth;
                    xD = 1;
                }
            }
            if (xD == 1) {
                ownLeft -= i;
                own.css({
                    left: ownLeft
                });
                if (ownLeft <= 0) xD = 0;
            }
            if (yD == 0) {
                ownTop += i;
                own.css({
                    top: ownTop
                });
                if (ownTop >= winHeight) {
                    ownTop = winHeight;
                    yD = 1;
                }
            }
            if (yD == 1) {
                ownTop -= i;
                own.css({
                    top: ownTop
                });
                if (ownTop <= 0) yD = 0;
            }
        }
        var imgHover = setInterval(imgPosition, settings.speed);
        own.hover(function () {
            clearInterval(imgHover);
        },
            function () {
                imgHover = setInterval(imgPosition, settings.speed);
            });
    };
    /* cookie: get, set, delete a cookie to document.
     */
    $.cookie = function (name, value, options) {
        if (typeof value != 'undefined') {
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString();
            }
            var path = options.path ? '; path=' + options.path : '';
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        } else {
            var cookieValue = null;
            if (document.cookie && document.cookie != '') {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var cookie = $.trim(cookies[i]);
                    if (cookie.substring(0, name.length + 1) == (name + '=')) {
                        cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                        break;
                    }
                }
            }
            return cookieValue;
        }
    };
})(jQuery);

// $.fn.echart: Use ECharts.
(function ($) {
    function load(target) {
        var state = $.data(target, 'echart');
        var chart = echarts.init(target);
        var opts = state.options;
        if (opts.url) {
            AX.ajax({
                type: opts.type,
                url: opts.url,
                data: opts.param,
                cb: function (data) {
                    var o = opts.option;
                    if (opts.dataset) {
                        o.dataset = { source: data.value };
                    }
                    else if (o.series instanceof Array) {
                        o.series[0].data = data.value;
                    }
                    else {
                        o.series.data = data.value;
                    }
                    chart.setOption(o);
                }
            });
        }
    }    

    $.fn.echart = function (options, param) {
        if (typeof options == 'string') {
            return $.fn.echart.methods[options](this, param);
        }
        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'echart');
            if (state) {
                $.extend(state.options, options);
            } else {
                state = $.data(this, 'echart', {
                    options: $.extend({}, $.fn.echart.defaults, $.fn.echart.parseOptions(this), options)
                });
            }
            load(this);
        });
    };

    $.fn.echart.methods = {
        options: function (jq) {
            return $.data(jq[0], 'echart').options;
        },
        reload: function (jq) {
            return jq.each(function () {
                load(this);
            });
        } 
    };

    $.fn.echart.parseOptions = function (target) {
        var t = $(target);
        return $.extend({}, $.parser.parseOptions(target, []), {});
    };

    $.fn.echart.defaults = {
    };
})(jQuery);

// $.fn.kindeditor: Use KindEditor.
(function ($) {
    function load(target) {
        var opts = $.data(target, 'kindeditor').options;
        KindEditor.ready(function (K) {
            opts.editor = K.create('#' + target.id, opts);
        });
    }

    $.fn.kindeditor = function (options, param) {
        if (typeof options == 'string') {
            return $.fn.kindeditor.methods[options](this, param);
        }

        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'kindeditor');
            if (state) {
                $.extend(state.options, options);
            } else {
                state = $.data(this, 'kindeditor', {
                    options: $.extend({}, $.fn.kindeditor.defaults, $.parser.parseOptions(this), options)
                });
            }
            load(this);
        });
    };

    $.fn.kindeditor.methods = {
        options: function (jq) {
            return $.data(jq[0], 'kindeditor').options;
        },
        sync: function (jq) {
            return jq.each(function () {
                $.data(this, 'kindeditor').options.editor.sync();
            });
        }
    };

    $.fn.kindeditor.defaults = {
        minWidth: '400px',
        minHeight: '150px',
        basePath: '/scripts/kindeditor/',
        pasteType: 1,
        allowFileManager: true,
        uploadJson: '/api/file/upload',
        fileManagerJson: '/api/file/getkind',
        filePostName: 'file',
        items: ['fullscreen', 'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'removeformat', 'emoticons', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist', 'insertunorderedlist', '|', 'image', 'link', 'table', 'wordpaste'],
        cssData: '.ke-content p {text-indent:2em;line-height:150%;padding:0.2em 0;}',
        filterMode: false
    };
})(jQuery);

// $.fn.iframe.
(function ($) {
    $.fn.iframe = function (options, param) {
        if (typeof options == 'string') {
            return $.fn.iframe.methods[options](this, param);
        }

        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'iframe');
            if (state) {
                $.extend(state.options, options);
            } else {
                state = $.data(this, 'iframe', {
                    options: $.extend({}, $.fn.iframe.defaults, $.parser.parseOptions(this), options)
                });
            }
        });
    };

    $.fn.iframe.methods = {
        options: function (jq) {
            return $.data(jq[0], 'iframe').options;
        },
        click: function (jq, cb) {
            return jq.each(function () {
                AX.iframe.track(this, cb);
            });
        }
    };

    $.fn.iframe.defaults = {
    };
})(jQuery);