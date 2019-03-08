// JQueryUi Exten.

// $.fn.form
$.fn.form.defaults.success = null;

// $.fn.monthbox.
$.fn.monthbox = {};
$.fn.monthbox.formatter = function (d) {
    return AX.datestr(d, 'yyyy-MM');
}
$.fn.monthbox.parser = function (s) {
    var t = Date.parse(s);
    if (!isNaN(t)) {
        return new Date(t);
    } else {
        return new Date();
    }
}
$.fn.monthbox.onShowPanel = function () {
    var self = this;
    var c = $(self).datebox('calendar');
    var span = c.find('span.calendar-text');
    span.trigger('click');

    setTimeout(function () {
        var tds = c.find('div.calendar-menu-month-inner td');
        tds.click(function (e) {
            e.stopPropagation();
            var year = /\d{4}/.exec(span.html())[0];
            var month = parseInt($(this).attr('abbr'), 10);
            $(self).datebox('hidePanel').datebox('setValue', year + '-' + month);
        });
    }, 0)
}

// $.messager
$.extend($.messager, {
    bodyStyle: 'padding:3px 8px 5px;border:1px solid #e2d526;background-color:#fcf495;font-weight:bold;overflow:hidden',
    /* info: success information on the top of page.
     * param in: msg
     */
    info: function (opts) {
        var width = AX.len(opts.msg) * 7;
        if (width < 300) width = 300;
        if (width > AX.opts.maxW) width = AX.opts.maxW - 100;
        window.top.$.messager.show($.extend({
            bodyStyle: $.messager.bodyStyle + ";color:green",
            height: 30,
            width: width,
            showType: 'fade',
            timeout: 3000,
            style: { top: 10, padding: 0, border: 0, position: 'fixed' }
        }, opts));
    },
    /* error: error information on the top of page.
     * param in: msg
     */
    error: function (opts) {
        $.messager.info($.extend(opts, {
            bodyStyle: $.messager.bodyStyle + ";color:red",
            timeout: 4000
        }));
    }
});

// validatebox
$.extend($.fn.validatebox.defaults.rules, {
    equals: {
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '输入的字符不一致'
    },
    uid: {
        validator: function (value, param) {
            if (value.length <= param[1] && value.length >= param[0]) {
                return new RegExp("^(?!_)(?!.*?_$)[a-zA-Z0-9_]+$").test(value);
            }
            return false;
        },
        message: '输入4-20位的字母、数字和下划线(不能开头和结尾)'
    }
});

// $.fn.combobox
$.extend($.fn.combobox.methods, {
    /* setIndex: select item by index.
     */
    setIndex: function (jq, index) {
        if (!index) index = 0;
        var data = jq.combobox('getData');
        var field = $(jq).combobox('options').valueField;
        jq.combobox('select', data[index][field]);
    },
    /* getIndex: get selected item's index.
     */
    getIndex: function (jq) {
        return jq.combobox('panel').find('.combobox-item-selected').index();
    }
});

// $.fn.tree
$.extend($.fn.tree.defaults, {
    /* onLoadSuccess: selected prev node after reloaded data.
     */
    onLoadSuccess: function (node, data) {
        if (AX.opts._curTreeId != null) {
            var t = $(this).tree('find', AX.opts._curTreeId).target;
            $(this).tree('select', t);
            $(this).tree('scrollTo', t);
        }
        var opts = $(this).tree('options');
        opts.success.call(this, node, data);
    },
    /* success: [+] added for parse content button after loading.
     */
    success: function (node, data) {
    }
});
$.extend($.fn.tree.methods, {
    /* getLevel: return level or the level's node when error msg when null.
     * target as: 
     * 1. underfined: selectedLevel(n), null(0)
     * 2. number: selectedLevelNode(n), leaf(0), rootElse(-1)
     * 3. element: level(n)
     */
    getLevel: function (jq, target) {
        if (typeof target == 'undefined') {
            var node = jq.tree('getSelected');
            if (node == null) return 0;
            else return jq.tree('getLevel', node.target);
        }
        else if (typeof target == 'number') {
            var isValid = true, node = jq.tree('getSelected');
            var index = target < 0 ? 3 : (target > 1 ? 2 : target);
            var msg = ['最底层', '根', AX.format('第{0}层', target), '非根'][index];
            if (node == null) isValid = false;
            else if (target == 0) isValid = jq.tree('isLeaf', node.target);
            else if (target == -1) isValid = jq.tree('getLevel', node.target) != 1;
            else isValid = jq.tree('getLevel', node.target) == target;
            if (!isValid) {
                $.messager.error({ msg: AX.format('请选择树{0}节点后再执行操作！', msg) });
                return null;
            }
            return node;
        } else {
            return $(target).find('span.tree-indent,span.tree-hit').length;
        }
    },
    /* getPrev: get previous node of current selected node.
     */
    getPrev: function (jq, target) {
        var t = $(target).parent().prev().children();
        return t.length ? jq.tree('getNode', t[0]) : null;
    },
    /* getNext: get next node of current selected node.
     */
    getNext: function (jq, target) {
        var t = $(target).parent().next().children();
        return t.length ? jq.tree('getNode', t[0]) : null;
    },
    /* getCheckedId: return checked node's id.
     * idField as: if underfined return node.id else return idField's value.
     */
    getCheckedId: function (jq, idField) {
        var id, ids = '', cks = jq.tree('getChecked');
        for (var i = 0; i < cks.length; i++) {
            id = idField ? cks[i].attributes[idField] : cks[i].id;
            if (id != '') ids += ',' + id;
        }
        if (ids == '') $.messager.error({ msg: '请选择树节点后再执行操作！' });
        else ids = ids.substr(1);
        return ids;
    },
    /* getSelectedId: return the levelSelectedNode's id.
     * param in: level, idField
     */
    getSelectedId: function (jq, opts) {
        var node, opts = opts || {};
        if (opts.level == 0 || opts.level) node = jq.tree('getLevel', opts.level);
        else {
            node = jq.tree('getSelected');
            if (node == null) $.messager.error({ msg: '请选择树节点后再执行操作！' });
        }
        if (node != null) {
            AX.opts._curTreeId = node.id;
            if (opts.editUrl) {
                opts.editUrl = AX.objstr(opts.editUrl, node.attributes);
            }
            return opts.idField ? node.attributes[opts.idField] : node.id;
        }
        return null;
    },
    /* add: show add dialog for tree.
     * param in: level, idField, [editUrl]
     */
    add: function (jq, opts) {
        opts = $.extend({}, jq.tree('options'), opts);
        var parId = jq.tree('getSelectedId', opts);
        if (parId == null) return;
        var url = opts.editUrl || 'edit';
        if (url.indexOf('?') < 0) url += '?parentid=' + parId;
        else url = AX.aurl(url, "parentid", parId);
        if (opts.q) url += "&" + opts.q;
        if (AX.opts.dialogMode) {
            AX.dialog('添加', url, function () {
                $.messager.info({ msg: '添加成功！' });
                jq.tree('reload');
            }, opts.w ? opts.w : AX.opts.w, opts.h ? opts.h : AX.opts.h);
        } else {
            AX.load(url);
        }
    },
    /* edit: show edit dialog for tree.
     * param in: level, idField, [editUrl]
     */
    edit: function (jq, opts) {
        opts = $.extend({}, jq.tree('options'), opts);
        var id = jq.tree('getSelectedId', opts);
        if (id == null) return;
        var url = AX.format("{0}?id={1}", (opts.editUrl || 'edit'), id);
        if (opts.q) url += "&" + opts.q;
        if (AX.opts.dialogMode) {
            AX.dialog('编辑', url, function () {
                $.messager.info({ msg: '保存成功！' });
                jq.tree('reload');
            }, opts.w ? opts.w : AX.opts.w, opts.h ? opts.h : AX.opts.h);
        } else {
            AX.load(url);
        }
    },
    /* del: delete current selected node for tree.
     * param in: level, idField, [delApi, query]
     */
    del: function (jq, opts) {
        opts = $.extend({}, jq.tree('options'), opts);
        var id = jq.tree('getSelectedId', opts);
        if (id == null) return;
        var t = jq.tree('getSelected').target;
        var next = jq.tree('getNext', t);
        if (next != null) {
            AX.opts._curTreeId = next.id;
        } else {
            var prev = jq.tree('getPrev', t);
            if (prev != null) {
                AX.opts._curTreeId = prev.id;
            } else {
                AX.opts._curTreeId = jq.tree('getParent', t).id;
            }
        }
        $.messager.confirm('提示', '确定要【删除】选中的节点？', function (r) {
            if (!r) return;
            AX.ajax({
                url: AX.api({
                    api: opts.delApi || 'crud/delete',
                    ds: opts.ds || opts.delDs,
                    q: 'id=' + id
                }),
                type: 'delete',
                cb: function () {
                    $.messager.info({ msg: '删除成功！' });
                    jq.tree('reload');
                }
            });
        });
    }
});

// $.fn.datagrid
$.extend($.fn.datagrid.defaults, {
    /* onLoadSuccess: parse panel's content to aceui-button.
     */
    onLoadSuccess: function (data) {
        var dg = $(this), opts = dg.datagrid('options');
        // export
        if (opts.export) {
            AX.gridTb(dg);
        }

        // render actions.
        dg.datagrid('clearChecked');
        dg.datagrid('getPanel').find('.easyui-linkbutton').linkbutton({ plain: true });
        dg.datagrid('getPanel').find('.easyui-tooltip').tooltip();

        // merged cells.
        var fields = dg.datagrid('getColumnFields'), rs = [];
        for (var c = 0; c < fields.length; c++) {
            var col = fields[c];
            rs.push([]);
            if (dg.datagrid('getColumnOption', col).merged) {
                var cur = data.rows[0][col], row = 0, rowspan = 1;
                var prevRs = 0, ix = 0;
                if (c > 0) {
                    prevRs = rs[c - 1][ix++];
                }
                for (var r = 1; r < data.rows.length; r++) {
                    if (cur != data.rows[r][col] || (r == prevRs)) {
                        if (r == prevRs) {
                            prevRs += rs[c - 1][ix++];
                        }
                        cur = data.rows[r][col];
                        if (rowspan > 1) {
                            dg.datagrid('mergeCells', { index: row, field: col, rowspan: rowspan });
                            rs[c].push(rowspan);
                            rowspan = 1;
                        }
                        else {
                            rs[c].push(1);
                        }
                        row = r;
                    }
                    else {
                        rowspan++;
                    }
                }
                if (rowspan > 1) {
                    dg.datagrid('mergeCells', { index: row, field: col, rowspan: rowspan });
                    rs[c].push(rowspan);
                }
            }
        }

        // resize column width.
        dg.datagrid('resize');
        //dg.datagrid('fixColumnSize');
        if (dg.datagrid('getColumnOption', 'action')) {
            dg.datagrid('autoSizeColumn', 'action');
        }
        opts.success.call(this, data);
    },
    /* success: [+] added event after loading.
     */
    success: function (data) {
    },
    /* onAceEdit: [+] added for datagrid's edited.
     */
    onAceEdit: function (id) {
    },
    /* onAceEdit: [+] added for datagrid's deleted.
     */
    onAceDelete: function (id) {
    }
});
$.extend($.fn.datagrid.methods, {
    /* getRow: get row object by idValue.
     */
    getRow: function (jq, idValue) {
        if (typeof idValue == 'undefined') {
            return jq.datagrid('getSelected');
        }
        var data = jq.datagrid('getRows');
        var row = jq.datagrid('getRowIndex', idValue);
        return data[row];
    },
    /* get: return checked row's id
     */
    getCheckedId: function (jq, idField) {
        var id, ids = '', cks = jq.datagrid('getChecked');
        for (var i = 0; i < cks.length; i++) {
            id = idField ? cks[i][idField] : cks[i].id;
            if (id != '') ids += ',' + id;
        }
        if (ids == '') $.messager.error({ msg: "请选择记录后再执行批量操作！" });
        else ids = ids.substr(1);
        return ids;
    },
    /* add: show add dialog for datagrid.
     * param in: [editUrl, query]
     */
    add: function (jq, query) {
        var o = jq.datagrid('options');
        var url = o.editUrl || 'edit';
        if (query) url += '?' + query;
        if (AX.opts.dialogMode) {
            AX.dialog('添加', url, function () {
                $.messager.info({ msg: '添加成功！' });
                jq.datagrid('reload');
            }, o.w ? o.w : AX.opts.w, o.h ? o.h : AX.opts.h);
        } else {
            AX.load(url);
        }
    },
    /* edit: show edit dialog for datagrid.
     * param in: id, [editUrl]
     */
    edit: function (jq, id) {
        var o = jq.datagrid('options');
        var url = AX.format("{0}?id={1}", (o.editUrl || 'edit'), id);
        if (AX.opts.dialogMode) {
            AX.dialog('编辑', url, function () {
                $.messager.info({ msg: '保存成功！' });
                jq.datagrid('reload');
                if (o.onAceEdit) {
                    o.onAceEdit.call(jq, id);
                }
            }, o.w ? o.w : AX.opts.w, o.h ? o.h : AX.opts.h);
        } else {
            AX.load(url);
        }
    },
    /* del: delete checked rows for datagrid.
     * param in: [id, delApi, query]
     */
    del: function (jq, id) {
        var o = jq.datagrid('options');
        if (typeof id == 'undefined') id = jq.datagrid('getCheckedId');
        if (id == '') return;
        var op = o.delTip || '删除';
        $.messager.confirm('提示', '确定要【' + op + '】选中的记录？', function (r) {
            if (!r) return;
            AX.ajax({
                url: AX.api({
                    api: o.delApi || 'crud/delete',
                    ds: o.delDs,
                    q: 'id=' + id
                }),
                type: 'delete',
                cb: function () {
                    $.messager.info({ msg: op + '成功！' });
                    jq.datagrid('reload');
                    if (o.onAceDelete) {
                        o.onAceDelete.call(jq, id);
                    }
                }
            });
        });
    }
});

// $.fn.treegrid
$.extend($.fn.treegrid.defaults, {
    /* onBeforeLoad: [o] override for clear queryParams'id when reloading data.
     */
    onBeforeLoad: function (row, param) {
        if (!row) param.id = '';
    },
    /* onLoadSuccess: parse panel's content to aceui-button.
     */
    onLoadSuccess: function (row, data) {
        var dg = $(this), opts = dg.datagrid('options');
        dg.treegrid('clearChecked');
        dg.treegrid('getPanel').find('.easyui-linkbutton').linkbutton({ plain: true });
        dg.treegrid('resize');
        //dg.treegrid('fixColumnSize');
        dg.treegrid('autoSizeColumn', 'action');
        opts.success.call(this, row, data);
    },
    /* success: [+] added for parse content button after loading.
     */
    success: function (row, data) {
    }
});
$.extend($.fn.treegrid.methods, {
    /* getRow: get row object by idValue.
     */
    getRow: function (jq, idValue) {
        if (typeof idValue == 'undefined') {
            return jq.treegrid('getSelected');
        }
        return jq.treegrid('find', idValue);
    },
    /* get: return checked row's id
     */
    getCheckedId: function (jq, idField) {
        var id, ids = '', cks = jq.treegrid('getChecked');
        for (var i = 0; i < cks.length; i++) {
            id = idField ? cks[i][idField] : cks[i].id;
            if (id != '') ids += ',' + id;
        }
        if (ids == '') $.messager.error({ msg: "请选择记录后再执行批量操作！" });
        else ids = ids.substr(1);
        return ids;
    },
    /* add: show add dialog for treegrid.
     * param in: [editUrl, query]
     */
    add: function (jq, query) {
        var o = jq.treegrid('options');
        var url = o.editUrl || 'edit';
        if (query) url += '?' + query;
        if (AX.opts.dialogMode) {
            AX.dialog('添加', url, function () {
                $.messager.info({ msg: '添加成功！' });
                jq.treegrid('reload');
            }, o.w ? o.w : AX.opts.w, o.h ? o.h : AX.opts.h);
        } else {
            AX.load(url);
        }
    },
    /* edit: show edit dialog for treegrid.
     * param in: id, [editUrl]
     */
    edit: function (jq, id) {
        var o = jq.treegrid('options');
        var url = AX.format("{0}?id={1}", (o.editUrl || 'edit'), id);
        if (AX.opts.dialogMode) {
            AX.dialog('编辑', url, function () {
                $.messager.info({ msg: '保存成功！' });
                jq.treegrid('reload');
            }, o.w ? o.w : AX.opts.w, o.h ? o.h : AX.opts.h);
        } else {
            AX.load(url);
        }
    },
    /* del: delete checked rows for treegrid.
     * param in: [id, delApi, query]
     */
    del: function (jq, id) {
        var o = jq.treegrid('options');
        if (typeof id == 'undefined') id = jq.treegrid('getCheckedId');
        if (id == '') return;
        var op = o.delTip || '删除';
        $.messager.confirm('提示', '确定要【' + op + '】选中的记录？', function (r) {
            if (!r) return;
            AX.ajax({
                url: AX.api({
                    api: o.delApi || 'crud/delete',
                    ds: o.delDs,
                    q: 'id=' + id
                }),
                type: 'delete',
                cb: function () {
                    $.messager.info({ msg: op + '成功！' });
                    jq.treegrid('reload');
                }
            });
        });
    }
});