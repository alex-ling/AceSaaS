// $.fn.uploadbox: Use Plupload.
(function ($) {
    function create(target) {
        var id = target.id, owner = $(target).parent();
        var state = $.data(target, 'uploadbox');
        var opts = $.extend({ id: id, browse_button: 'btn_' + id }, state.options);
        var html = AX.format(
            '<div class="upload-wrapper">\
            <div class="btn-wrapper">\
            <input type="button" value=" 选择文件 " id="btn_{0}" />\
            <b class="ml5">{2}单个不要超过{1}</b>\
            </div>\
            <div class="file-wrapper"><div class="clear"></div></div>\
            </div>', id, opts.filters.max_file_size, opts.picView ? '' : '最多上传' + opts.max + '个文件，');
        $(html).insertAfter(target);
        var up = new plupload.Uploader(opts);
        state.options._uploader = up;
        up._opts = opts;
        up._box = $(target);
        up._files = $('.file-wrapper', owner);
        up.init();
        if (!opts.picView) {
            $('#btn_' + id).click(function () {
                var cur = up._files.children().length - 1;
                if (cur >= up._opts.max) {
                    $.messager.error({ msg: "最多上传" + up._opts.max + "个文件" });
                    up.disableBrowse(true);
                }
                else {
                    up.disableBrowse(false);
                }
            });
        }
    }
    function getParams(up) {
        var now = AX.tick();
        if (!up.params || up.params.expire < now + 3) {
            AX.ajax({
                url: AX.api({ api: 'cloud/getosssign' }),
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
        if (up._opts.picView) {
            up._files.find("#file-1").remove();
            var having = false;
            up._files.find("div.file").each(function () {
                $(this).find("a.fr").click();
                having = true;
            });
            if (having) return;
        }
        var html = AX.format(
            '<div id="file-{0}" class="file" style="{4}">{5}\
            <div class="file-name" style="{6}">\
            <a class="lk" target="_blank">{1}</a>\
            <span>{2}</span>\
            <b>{3}%</b>\
            <a class="fr" href="javascript:;" style="{6}">删除</a>\
            <div class="clear"></div>\
            </div>\
            <div class="progress" style="{6}">\
            <div class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100"></div>\
            </div>\
            </div>',
            file.id,
            file.name,
            file.size > 0 ? '(' + plupload.formatSize(file.size) + ')' : '',
            file.percent || 0,
            up._opts.picView ? AX.format('float:left;width:{0}px', up._opts.picWidth) : '',
            up._opts.picView ? AX.format('<img style="width:100%;height:{0}px;border:1px solid #ccc" />', up._opts.picHeight) : '',
            up._opts.picView && file.url == "/images/none.jpg" ? 'display:none' : ''
        );
        $(html).insertBefore(up._files.find('.clear').last()).find('a.fr').click(function () {
            if (file.percent >= 100) {
                if (up._opts.bucket) {
                    AX.ajax({
                        url: AX.api({ api: 'cloud/delossfile' }),
                        data: { bucket: up._opts.bucket, key: file.ossName },
                        type: 'delete',
                        cb: function () {
                            up._box.val(up._box.val().replace("," + file.url, ""));
                        }
                    });
                }
                else if (up._opts.dir) {
                    AX.ajax({
                        url: AX.api({ api: 'file/deletefile' }),
                        data: { id: file.url },
                        type: 'delete',
                        cb: function () {
                            up._box.val(up._box.val().replace("," + file.url, ""));
                        }
                    });
                }
            }
            if (file.size > 0) up.removeFile(file);
            $(this).closest('.file').slideUp(1000, function () {
                $(this).remove();
                if (up._opts.picView) {
                    if (up.files.length) {
                        createProgress(up, up.files[0]);
                    }
                    else {
                        createProgress(up, {
                            id: '1',
                            name: 'none',
                            ossName: '',
                            url: '/images/none.jpg',
                            size: 0,
                            percent: 100
                        });
                    }
                }
            });
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
            PostInit: function (up) {
                var files = this._box.val().split(',');
                if (up._opts.picView && files.length < 2) {
                    files.push('/images/none.jpg');
                }
                for (var i = 1; i < files.length; i++){
                    var path = files[i];
                    var name = AX.filename(path).substr(11);
                    var ossName = path;
                    if (path.substr(0, 4) == 'http') {
                        ossName = path.substr(path.indexOf('/', 10) + 1);
                    }
                    createProgress(this, {
                        id: up._opts.id + i,
                        name: name,
                        ossName: ossName,
                        url: path,
                        size: 0,
                        percent: 100
                    });
                }
            },
            FilesAdded: function (up, files) {
                var cur = up._files.children().length - 1;
                var removeFiles = 0;
                plupload.each(files, function (file) {
                    if (!up._opts.picView) {
                        if (cur < up._opts.max) {
                            createProgress(up, file);
                        }
                        else {
                            removeFiles++;
                            up.removeFile(file);
                        }
                    }
                    else {
                        createProgress(up, file);
                    }
                });
                if (up._opts.autoUpload) {
                    setTimeout(function () { up.start(); }, 100);
                }
                if (removeFiles > 0) {
                    $.messager.error({ msg: "最多上传" + up._opts.max + "个文件，已移除队尾" + removeFiles + "个文件" });
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
                            file.url = JSON.parse(info.response).value;
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