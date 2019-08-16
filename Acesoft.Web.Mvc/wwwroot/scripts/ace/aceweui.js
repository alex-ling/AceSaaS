// $.fn.uploadbox: Use Plupload.
(function ($) {
    function create(target) {
        var id = target.id, owner = $(target).parent();
        var state = $.data(target, 'uploadbox');
        var opts = $.extend({ browse_button: 'btn_' + id }, state.options);
        var html = AX.format('<div id="btn_{0}" class="weui-uploader__input-box"></div>', id);
        $(html).insertAfter(target);

        var up = new plupload.Uploader(opts);
        state.options._uploader = up;
        up._opts = opts;
        up._box = $(target);
        up._container = $(target).parent();
        up._files = $(target).closest('.weui-uploader').find('.weui-uploader__files');
        up._gallery = $(target).closest('.weui-cells').find('.weui-gallery');
        up._numbox = $(target).closest('.weui-uploader').find('.num');
        up._files.on("click", "li", function () {
            up._index = $(this).index();
            up._gallery.find('.weui-gallery__img').attr("style", this.getAttribute("style"));
            up._gallery.fadeIn(100);
        });
        up._files.on("click", ".close", function(event) {
            event.stopPropagation();
            up._index = $(this).parent().index();
            var id = up._files.find("li").eq(up._index).attr("id");
            removeFile(up, up.getFile(id));
        });
        up._gallery.on("click", function () {
            up._gallery.fadeOut(100);
        });
        up._gallery.find(".weui-gallery__del").click(function() {
            var id = up._files.find("li").eq(up._index).attr("id");
            removeFile(up, up.getFile(id));
        });
        up.init();
    }
    function getOssSign(up) {
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
    function addFile(up, file) {
        var r = new moxie.file.FileReader();
        r.onload = function () {
            var html = AX.format('<li id="{0}" class="weui-uploader__file weui-uploader__file_status" style="background-image:url({1})">\
                <div class="close">X</div>\
                <div class="weui-uploader__file-content">0%</div>\
            </li>', file.id, r.result);
            up._files.append($(html));
            r.destroy();
            f = null;
        }
        r.readAsDataURL(file.getSource());
    }
    function removeFile(up, file) {
        if (!file) return;
        if (file.url) {
            if (up._opts.bucket) {
                AX.ajax({
                    url: AX.api({ api: 'aliyun/delossfile' }),
                    data: { bucket: up._opts.bucket, key: file.ossName },
                    type: 'delete',
                    cb: function() {
                        up._box.val(up._box.val().replace("," + file.url, ""));
                        removeEle(up, file);
                    }
                });
            }
            else if (up._opts.dir) {
                AX.ajax({
                    url: AX.api({ api: 'file/delfile' }),
                    data: { file: file.url },
                    type: 'delete',
                    cb: function() {
                        up._box.val(up._box.val().replace("," + file.url, ""));
                        removeEle(up, file);
                    }
                });
            }
        }
        else {
            removeEle(up, file);
        }
    }
    function removeEle(up, file) {
        up.removeFile(file);
        up._files.find("li").eq(up._index).remove();
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
                    options: $.extend({}, $.fn.uploadbox.defaults, options)
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
        },
        count: function(jq) {
            var opts = $.data(jq[0], 'uploadbox').options;
            return opts._uploader.files.length;
        },
        fadeout: function(jq) {
            var opts = $.data(jq[0], 'uploadbox').options;
            opts._uploader._gallery.fadeOut(100);
        }
    };

    $.fn.uploadbox.defaults = {
        url: 'http://oss.aliyuncs.com',
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
                for (var i = 1; i < files.length; i++) {
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
            FilesAdded: function(up, files) {
                var count = up.files.length - files.length;
                var having = false;
                plupload.each(files, function(file) {
                    if (count++ < up._opts.max) {
                        addFile(up, file);
                    }
                    else {
                        having = true;
                        up.removeFile(file);
                    }
                });
                if (having) {
                    AX.wxWarn('最多能上传' + up._opts.max + '个文件！');
                }
                if (up.files.length == up._opts.max) {
                    up._container.hide();
                }
                up._numbox.text(up.files.length);
                if (up._opts.autoUpload) {
                    setTimeout(function () { up.start(); }, 100);
                }
            },
            FilesRemoved: function(up, files) {
                if (up.files.length < up._opts.max) {
                    up._container.show();
                }
                up._numbox.text(up.files.length);
            },
            BeforeUpload: function (up, file) {
                if (up._opts.bucket) {
                    var params = getOssSign(up);//expire
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
                else {
                    if (up._opts.dir) {
                        file.fileName = up._opts.dir + '/' + AX.tick() + '_' + file.name;
                        file.url = '/upload/' + file.fileName;
                    }
                    if (up._opts.beforeUpload) {
                        up._opts.beforeUpload.call(up);
                    }
                }
            },
            UploadProgress: function (up, file) {
                $('#' + file.id).find('.weui-uploader__file-content').text(file.percent + '%');
            },
            FileUploaded: function (up, file, info) {
                if (info.status == 200) {
                    //回调成功 info.response;
                    if (up._opts.dir) {
                        up._box.val(up._box.val() + ',' + file.url);
                    }
                }
                else if (info.status == 203) {
                    //回调失败 info.response;
                }
            },
            UploadComplete: function(up, files) {
                if (up._opts.uploaded) {
                    up._opts.uploaded.call();
                }
            },
            Error: function (up, err) {
                if (err.code == -600) {
                    AX.wxWarn("选择的文件太大了！");
                }
                else if (err.code == -601) {
                    AX.wxWarn("选择的文件后缀不对！");
                }
                else if (err.code == -602) {
                    AX.wxWarn("这个文件已经上传!");
                }
                else {
                    AX.wxWarn("发生错误:" + err.response);
                }
            }
        }
    };
})(jQuery);