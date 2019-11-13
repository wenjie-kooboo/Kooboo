(function() {
  new Vue({
    el: "#app",
    data: function() {
      return {
        text: {
          transferTask: Kooboo.text.common.TransferTask,
          sites: Kooboo.text.component.breadCrumb.sites,
          dashboard: Kooboo.text.component.breadCrumb.dashboard
        },
        tableData: [],
        tableDataSelected: []
      };
    },
    created: function() {
      this.breads = [
        {
          name: this.text.sites
        },
        {
          name: this.text.dashboard
        },
        {
          name: this.text.transferTask
        }
      ];
      this.getTableData();
    },

    methods: {
      getConfirmMessage: function(doc) {
        if (doc.relations) {
          var reorderedKeys = _.sortBy(Object.keys(doc.relations));
          doc.relationsTypes = reorderedKeys;
        }
        var find = _.find(doc, function(item) {
          return item.relations && Object.keys(item.relations).length;
        });

        if (!!find) {
          return Kooboo.text.confirm.deleteItemsWithRef;
        } else {
          return Kooboo.text.confirm.deleteItems;
        }
      },
      onDelete: function() {
        if (confirm(this.getConfirmMessage(this.tableDataSelected))) {
          var ids = this.tableDataSelected.map(function(m) {
            return m.id;
          });
          Kooboo[Kooboo.TransferTask.name]
            .Deletes({
              ids: ids
            })
            .then(function(res) {
              if (res.success) {
                window.info.done(Kooboo.text.info.enable.success);
                self.getDomainsData();
                self.cancelDialog();
              } else {
                window.info.fail(Kooboo.text.info.enable.failed);
              }
            });
        }
      },
      getTableData: function() {
        var vm = this;
        Kooboo.TransferTask.getList().then(function(res) {
          if (res.success) {
            vm.tableData = res.model.map(function(item) {
              var date = new Date(item.lastModified);
              return {
                id: item.id,
                url: item.fullStartUrl,
                date: date.toDefaultLangString()
              };
            });
          }
        });
      }
    }
  });
})();
