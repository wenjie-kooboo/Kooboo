if(!window.Kooboo){
    window.Kooboo={};
}
Kooboo.Vue={
    data:{},
    vueHooks:["created"],//kvmodel only need support created
    getMergeHooks:function(models){
        var mergeHooks={};
        Kooboo.Vue.vueHooks.forEach(function(hook){
            var hookFuncs=[];
            for(var i=models.length-1;i>=0;i--){
                var model=models[i];
                if(model[hook]){
                    hookFuncs.push(model[hook]);
                }
            }
            if(models.length>0 &&hookFuncs.length>0){
                //mergeHooks[hook]=hookFuncs;
                mergeHooks[hook]=function(){
                    hookFuncs.forEach(function(hookFunc){
                        hookFunc();
                    });
                }
            }
        })
        return mergeHooks;
    },
    getVueData:function(data){
        var vueData={};
        var models=data;
        if(models){
            var mergeHooks =Kooboo.Vue.getMergeHooks(models);
            for(var i=models.length-1;i>=0;i--){
                $.extend(true,vueData,models[i]);
            }
            if(mergeHooks&&Object.keys(mergeHooks).length>0){
                $.extend(true,vueData,mergeHooks);
            }
            
            if(Vue.resetValid){
                Vue.resetValid(vueData)
            }
            
        }
        return vueData;
    }
}

Vue.kExtend=function(model){
   if(model.el){
    var models=Kooboo.Vue.data[model.el];
    if(!models){
        models=[];
    }
    models.push(model);
    Kooboo.Vue.data[model.el]=models;
   }
}

Vue.kExecute=function(){
    var data=Kooboo.Vue.data;
    if(data&&Object.keys(data).length>0){
        var keys=Object.keys(data);
        for(var i=0;i<keys.length;i++){
            var value=data[keys[i]];
            var vueData= Kooboo.Vue.getVueData(value);
            new Vue(vueData);
        }
    }
    
}