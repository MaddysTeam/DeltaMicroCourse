
//首页菜单的处理
$('#home_article').on('articleload', function(){

	A.Slider('#home_slider', {
		dots : 'right'
	});
	
	loginHandler.do();	
	
	var newdata = $native.cache('edn.newdata');
	
	var scroller = A.Refresh('#home_article',{
		onPullDown:function(){
	    	renderTemplate();
	    }
	});
	
	var renderTemplate = function(o){
		if(o){
			A.template('#new_template').renderReplace(o, function(){
				scroller.refresh();
			});
		}		
		
		if(!o) A.showMask();
		
		$util.ajax({
			url : $config.exmobiSevice+'/show_list?view=new',
			type : 'get',
			dataType : 'json',
			success : function(data){
				if(!o) A.hideMask();
				if(A.JSON.stringify(newdata)==A.JSON.stringify(data)) return;	
						
				$native.cache('edn.newdata', data);
				
				A.template('#new_template').renderReplace(data, function(el, h, o){					
					scroller.refresh();
				});
			},
			error : function(){
				A.hideMask();
			}
		});
		
		$('#home_article #refresh_time').html('更新于：'+new Date().toString());
	};
	
	renderTemplate(newdata);
});
//论坛菜单的处理
$('#bbs_article').on('articleshow', (function(){
	
	var renderTemplate = function(){
		var bbsmenu = $native.cache("edn.bbsmenu");
		if(bbsmenu){
			//$('#bbs_template').renderReplace(bbsmenu);
			A.template('#bbs_template').renderReplace(bbsmenu,function(el, h, o){
				//A.Component.lazyload(h);
				A.Scroll('#bbs_article',{}).refresh();
			});
		}
		if(!bbsmenu) A.showMask();
		$util.ajax({
			url : $config.exmobiSevice+'/bbs',
			type : 'get',
			dataType : 'json',
			success : function(data){
				if(!bbsmenu) A.hideMask();
				//判断是否有数据更新
				if(A.JSON.stringify(data)!=A.JSON.stringify(bbsmenu)){	
					A.template('#bbs_template').renderReplace(data, function(){						
						$native.cache("edn.bbsmenu",data);
						bbsmenu = $native.cache("edn.bbsmenu");
					});
				}
			},
			error : function(){
				A.hideMask();
			}
		});
		
		
	};
	
	return function(){
		renderTemplate();
	}
})());
//抢沙发菜单的处理
$('#sofa_article').on('refreshInit', function(){
	var sofadata = $native.cache('edn.sofadata');
	
	var scroller = A.Refresh('#sofa_article',{});
	
	scroller.on('pulldown', function() {
		renderTemplate();
	});
	
	var renderTemplate = function(d){
		if(d){
			A.template("#sofa_template").renderReplace(d, function(h){
				scroller.refresh();
			});
		}
		
		if(!d) A.showMask();
		$util.ajax({
			url : $config.exmobiSevice+'/show_list?view=sofa',
			type : 'get',
			dataType : 'json',
			success : function(data){
				if(!d) A.hideMask();
				if(A.JSON.stringify(data)!=A.JSON.stringify(sofadata)){		
					A.template("#sofa_template").renderReplace(data, function(){						
						$native.cache("edn.sofadata",data);
						scroller.refresh();
					});		
				}
			},
			error : function(){
				A.hideMask();
			}
		});

		$('#sofa_article #refresh_time').html('更新于：'+new Date().toString());
	};
	
    renderTemplate(sofadata);
	
});
//我的菜单的处理
$('#my_article').on('articleshow', function(){
	loginHandler.do();
});

	
//登录处理类封装
var loginHandler = (function(){
	var userinfoTempalate = {
    	"uid": "",
    	"result": "fail",
    	"tiezi": [],
    	"formhash": "",
    	"jinqian": "",
    	"zhuti": "",
    	"haoyou": "",
    	"jifen": ""
	};
	
	var userinfo = userinfoTempalate;
	
	var renderTemplate = function(obj){
		obj = obj||userinfoTempalate;
		$('#index_title').html(obj.username||'ExMobi');
		A.template('#my_template').renderReplace(obj, function(el, h, o){
			$('#logut').tap(function(){
				console.log("logut");
				$util.ajax({
					url : $config.exmobiSevice+'/logout',
					type : 'post',
					data : 'formhash='+obj.formhash,
					dataType : 'json',
					success : function(data){
						console.log(data);
						if(data.result=='success'){
							A.showToast('您已登出');
							$native.cache('edn.userinfo',null);
							$native.session('edn.userinfo', '');
							renderTemplate(userinfoTempalate);
							A.Controller.section("#index_section");
						}
					},
					error : function() {
						console.log("error");
					}
				});
				
			});
			
			$('#login').tap(function(){
				A.Controller.section('#login_section');
			});
			
			$('#qiandao').tap(function(){
				A.Controller.section('#qiandao_section');
			});
			
			//A.Component.lazyload(h);
			
			A.Scroll('#my_article',{}).refresh();
		});
	};
	
	var _do = function(isNew,cb){
		userinfo = $native.cache('edn.userinfo');
		var sessionUserinfo=$native.session('edn.userinfo');
		
		if(isNew){
			renderTemplate(userinfo||userinfoTempalate);
			cb&&cb();
		}else if(userinfo){
			$util.ajax({
				url : $config.exmobiSevice+'/check_login',
				type : 'post',
				data : 'username='+userinfo.username+'&password='+userinfo.password,
				dataType : 'json',
				success : function(data){
					if(data.result=='success'){					
						$native.session('edn.userinfo', data);
						$native.cache('edn.userinfo', data);			
						renderTemplate(data);
						cb&&cb();
					}
				}
			});
		}else if(sessionUserinfo){
			$util.ajax({
				url : $config.exmobiSevice+'/check_login',
				type : 'post',
				data : 'username='+sessionUserinfo.username+'&password='+sessionUserinfo.password,
				dataType : 'json',
				success : function(data){
					if(data.result=='success'){					
						$native.session('edn.userinfo', data);
						renderTemplate(data);
						cb&&cb();
					}
				}
			});
		}else{
			renderTemplate(userinfoTempalate);
			cb&&cb();
		}
	};

	return {
		do : _do,
		renderTemplate :　renderTemplate
	};
})();

//处理搜索框的所有操作
(function(){
	var $input = $('#search input');
	var $header = $('#header');
	var $search = $('#search');
	var _show = function(){
		$header.toggle();
		$input.val('');
	};
	var _hide = function(){
		$header.toggle();
	};
	$('#search_hide').tap(function(){
		A.anim.run($header,'slideDownIn',function(){
			_show();
		});
	});
	$('#search_show').on(A.options.clickEvent, function(){
		A.anim.run($header,'slideUpOut',function(){
			_hide();
		});
		return false;
	});
	
	$('#search_btn').on(A.options.clickEvent, function(){
		var val = $input.val().trim();
		if(val==''){
			A.alert("请输入搜索关键字");
			return;
		}
		
		A.anim.run($header,'slideDownIn',function(){
			A.Controller.section('search_section.html?key='+val);
			_show();
		});
		return false;
	});
	
	var _checkLogin = function(opt){
		loginHandler.do(false,function(){
			var userinfo = $native.session('edn.userinfo');
			if(!userinfo){
				//var itemsdata = $native.cache('edn.itemsdata');
				//var version = ClientUtil.getVersion();
				A.Controller.section('#login_section');
				/*
				if(itemsdata!=version){
					A.Controller.section('#items_section');
				}else{
					A.Controller.section('#login_section');
				}
				*/
				opt.fail&&opt.fail();
				return;
			}
			opt.success&&opt.success();
			return;
		});

	};
	
	
	$('#auction,#newpost,#chat').tap(function(){
		A.showMask();
		var thisId=this.id;
		_checkLogin({
			success:function(){
				A.hideMask();
				A.Controller.section('#'+thisId+"_section");
			},
			fail:function(){
				A.hideMask();
			}
		});
	});
	
})();
