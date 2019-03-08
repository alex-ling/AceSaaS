// $.fn.uploadbox: Use Plupload.
(function ($) {
    function create(target) {
        var id = target.id, owner = $(target).parent();
        var state = $.data(target, 'uploadbox');
        var opts = $.extend({ browse_button: 'btn_' + id }, state.options);
        var html = AX.format(
            '<div class="upload-wrapper">\
            <div class="btn-wrapper">\
            <input type="button" value=" 选择文件 " id="btn_{0}" />\
            <b class="ml5">单个文件不要超过{1}</b>\
            </div>\
            <div class="file-wrapper"><div class="clear"></div></div>\
            </div>', id, opts.filters.max_file_size);
        $(html).insertAfter(target);
        var up = new plupload.Uploader(opts);
        state.options._uploader = up;
        up._opts = opts;
        up._box = $(target);
        up._files = $('.file-wrapper', owner);
        up.init();
    }
    function getParams(up) {
        var now = AX.tick();
        if (!up.params || up.params.expire < now + 3) {
            AX.ajax({
                url: AX.api({ api: 'aliyun/getosssign' }),
                data: { bucket: up._opts.bucket, dir: up._opts.dir },
                type: 'get',
                async: false,
                cb: function (res) {
                    up.params = res.value;
                }
            });
        }
        return up.params;
    }
    function createProgress(up, file) {
        var html = AX.format(
            '<div id="file-{0}" class="file" style="{4}">{5}\
            <div class="file-name">\
            <a class="lk" target="_blank">{1}</a>\
            <span>{2}</span>\
            <b>{3}%</b>\
            <a class="fr" href="javascript:;">删除</a>\
            <div class="clear"></div>\
            </div>\
            <div class="progress">\
            <div class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100"></div>\
            </div>\
            </div>',
            file.id,
            file.name,
            file.size > 0 ? '（' + plupload.formatSize(file.size) + '）' : '',
            file.percent || 0,
            up._opts.picView ? AX.format('float:left;width:{0}px', up._opts.picWidth) : '',
            up._opts.picView ? AX.format('<img style="width:100%;height:{0}px" />', up._opts.picHeight) : ''
        );
        $(html).insertBefore(up._files.find('.clear').last()).find('a.fr').click(function () {
            if (file.percent >= 100) {
                if (up._opts.bucket) {
                    AX.ajax({
                        url: AX.api({ api: 'aliyun/delossfile' }),
                        data: { bucket: up._opts.bucket, key: file.ossName },
                        type: 'delete',
                        cb: function () {
                            up._box.val(up._box.val().replace("," + file.url, ""));
                        }
                    });
                }
                else if (up._opts.dir) {
                    AX.ajax({
                        url: AX.api({ api: 'file/delfile' }),
                        data: { file: '/wwwroot' + file.url },
                        type: 'delete',
                        cb: function () {
                            up._box.val(up._box.val().replace("," + file.url, ""));
                        }
                    });
                }
            }
            if (file.size > 0) up.removeFile(file);
            $(this).closest('.file').slideUp(1000, function () { $(this).remove() });
        });
        if (file.percent >= 100) {
            $('#file-' + file.id).find('.progress-bar')
                .attr('aria-valuenow', file.percent)
                .css('width', file.percent + '%')
                .end().find('img').attr('src', file.url)
                .end().find('.lk').attr('href', file.url);
        }
    }

    $.fn.uploadbox = function (options, param) {
        if (typeof options == 'string') {
            return $.fn.uploadbox.methods[options](this, param);
        }
        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'uploadbox');
            if (state) {
                $.extend(state.options, options);
            } else {
                state = $.data(this, 'uploadbox', {
                    options: $.extend({}, $.fn.uploadbox.defaults, $.fn.uploadbox.parseOptions(this), options)
                });
            }
            create(this);
        });
    };

    $.fn.uploadbox.methods = {
        options: function (jq) {
            return $.data(jq[0], 'uploadbox').options;
        },
        start: function (jq) {
            var opts = $.data(jq[0], 'uploadbox').options;
            opts._uploader.start();
        }
    };

    $.fn.uploadbox.parseOptions = function (target) {
        var t = $(target);
        return $.extend({}, $.parser.parseOptions(target, []), {});
    };

    $.fn.uploadbox.defaults = {
        url: '/api/file/upload',
        runtimes: 'html5,flash,silverlight,html4',
        multi_selection: true,
        flash_swf_url: 'plupload/js/Moxie.swf',
        silverlight_xap_url: 'plupload/js/Moxie.xap',
        multipart: true,
        autoUpload: true,
        max_retries: 0,
        beforeUpload: function () { },
        uploaded: function () { },
        filters: {
            mime_types: [
                { title: "图片文件", extensions: "jpg,jpeg,gif,png,bmp" },
                { title: "办公文档", extensions: "doc,docx,xls,xlsx,ppt,pptx,pdf,txt" },
                { title: "压缩文件", extensions: "zip,rar" }
            ],
            max_file_size: '20mb', //最大只能上传10mb的文件
            prevent_duplicates: true //不允许选取重复文件
        },
        init: {
            PostInit: function () {
                var files = this._box.val().split(',');
                for (var i = 1; i < files.length; i++){
                    var path = files[i];
                    var name = AX.filename(path).substr(11);
                    var ossName = path;
                    if (path.substr(0, 4) == 'http') {
                        ossName = path.substr(path.indexOf('/', 10) + 1);
                    }
                    createProgress(this, {
                        id: i,
                        name: name,
                        ossName: ossName,
                        url: path,
                        size: 0,
                        percent: 100
                    });
                }
            },
            FilesAdded: function (up, files) {
                plupload.each(files, function (file) {
                    createProgress(up, file);
                });
                if (up._opts.autoUpload) {
                    setTimeout(function () { up.start(); }, 100);
                }
            },
            BeforeUpload: function (up, file) {
                if (up._opts.bucket) {
                    var params = getParams(up);//expire
                    file.ossName = params.dir + '/' + AX.tick() + '_' + file.name;
                    file.url = params.host + '/' + file.ossName;
                    var mps = {
                        'key': file.ossName,
                        'policy': params.policy,
                        'OSSAccessKeyId': params.accessid,
                        'success_action_status': '200', // 让服务端返回200,不然，默认会返回204
                        'callback': params.callback,
                        'signature': params.signature,
                    };
                    up.setOption({
                        'url': params.host,
                        'multipart_params': mps
                    });
                }
                else if (up._opts.dir) {
                    up.setOption({ 'url': '/api/file/upload?folder=' + up._opts.dir })
                }
                if (up._opts.beforeUpload) {
                    up._opts.beforeUpload.call(up);
                }
            },
            UploadProgress: function (up, file) {
                $('#file-' + file.id).find('b').text(file.percent + '%')
                    .end().find('.progress-bar')
                    .attr('aria-valuenow', file.percent)
                    .css('width', file.percent + '%');
            },
            FileUploaded: function (up, file, info) {
                if (info.status == 200) {
                    //回调成功 info.response;
                    if (up._opts.dir) {
                        if (!up._opts.bucket) {
                            var res = JSON.parse(info.response).value;
                            file.url = res.url;
                        }
                        $('#file-' + file.id).find('a.lk').attr('href', file.url)
                            .end().find('img').attr('src', file.url);
                        up._box.val(up._box.val() + ',' + file.url);
                    }
                    if (up._opts.uploaded) {
                        up._opts.uploaded.call(up, file, info);
                    }
                }
                else if (info.status == 203) {
                    //回调失败 info.response;
                }
            },
            Error: function (up, err) {
                if (err.code == -600) {
                    $.messager.error({ msg: "选择的文件太大了！" });
                }
                else if (err.code == -601) {
                    $.messager.error({ msg: "选择的文件后缀不对！" });
                }
                else if (err.code == -602) {
                    $.messager.error({ msg: "这个文件已经上传!" });
                }
                else {
                    $.messager.error({ msg: "发生错误:" + err.response });
                }
            }
        }
    };
})(jQuery);