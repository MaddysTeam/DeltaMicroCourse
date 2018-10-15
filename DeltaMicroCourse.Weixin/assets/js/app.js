var courseItem = function (data) {
	this.imgSrc = data.ImagePath;
	this.videoSrc = data.VideoPath;
	this.videoId = data.VideoId;
	this.videoName = data.Name;
	this.schoolName = data.SchoolName;
	this.playCount = data.PlayCount;
	this.praiseCount = data.PraiseCount;
	this.linkUrl = data.LinkUrl;
	this.author = data.TeacherName;
}
courseItem.prototype.getTemplate = function (template, isLazy) {
	var o = this;
	var img = isLazy ? '<img style="width:65px;" alt="" data-source="' + o.imgSrc + '">' :
		                '<img style="width:65px;" alt="" src="' + o.imgSrc + '">';
	var str = template ||
	  '<li data-url='+o.linkUrl+'>' +
		 '  <div class="img appimg">' + img +
		 '	 </div>' +
		 '<div class="text">' +
		 '    <span>' + o.videoName + '</span>' +
		 '		<small>' +
					  o.schoolName +
		 '			<br>' +
		 '       作者：' + o.author + '<br>'+
		 '			播放：' + o.playCount + ' 点赞：' + o.praiseCount +
		 '		</small>' +
		 '	</div>' +
	 '</li>';
	return str;
}

var loader = function (url, current, searchType, searchTerm, categoryItemId) {
    this.url = url;
    this.current = current;
    this.searchType = searchType;
    this.searchTerm = searchTerm;
    this.categoryItemId = categoryItemId;
}
loader.prototype.loadAsync = function (callback) {
    var o = this;
    $.post(this.url, { current: o.current, searchType: o.searchType, searchTerm: o.searchTerm, categoryItemId: o.categoryItemId }, function (data) {
        o.current = data.current || 0;//记住当前pageindex
        if (typeof callback === 'function') {
            callback.call(o, data);
        }

    });
}

function bind(page, data, isLazy, templateFactory) {
	var content;
	if (!data || !data.rows || data.rows.length <= 0) {
		content = $("<h4>抱歉，没有检索到数据。</h4>");
		content.appendTo($(page));
	}
	else {
		var itemString = '';
		$(data.rows).each(function (index) {
			itemString += templateFactory ? templateFactory(this) : new courseItem(this).getTemplate(null, isLazy);
		});
		content = $(itemString);
		content.on(A.options.clickEvent, function (e) {
			location.href = $(this).attr('data-url');
		});
		content.appendTo($(page).find('.listitem'));
	}
	
   
}

function clear(page) {
    $(page).find('.listitem').empty();
}

function initialPage() {
    $('#article').on('articleload', function () {
        A.Slider('#sliderPage', {
            dots: 'hide'
        });
    });

}

function InitialSearchLink() {
	$('#searchLink').on(A.options.clickEvent, function (e) {
		location.href = $(this).data('url');
	});
}

initialPage();

var defaultColors = ["#3498db", "#F39C12", "#1abc9c", "#e74c3c", "#78ba00", "#8e44ad",
    "#f69c9f", "#f58f98", "#f58220", "#6a6da9", "#6d8346", "#56452d",
    "#ad8b3d", "#375830", "#d9d6c3", "#008792", "#f36c21", "#525f42",
];


var cplayer;
var objectId;
var playAction;

function videoPlay(player, cid, swfurl, playfunc, videoPath) {
	cplayer = player;
	objectId = 'ckplayer_' + cid;
	playAction = playfunc
	var flashvars = {
		f: videoPath,//视频地址
		loaded: 'loadHandler'
	};
	var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always' };//这里定义播放器的其它参数如背景色（跟flashvars中的b不同），是否支持全屏，是否支持交互
	var video = [videoPath];

   player.embed(swfurl, cid, objectId, '100%', '100%', false, flashvars, video, params);

}

function playHandler() {
	if (cplayer) {
		if (playAction && typeof playAction === "function") {
			playAction();
		}
	}
}

function loadHandler() {
	if (cplayer) {
		cplayer.getObjectById(objectId).addListener('play', playHandler);
	}

}




