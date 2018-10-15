//	pie charts
function PieChart($select, data, options) {

	var defaultOptions = {
		series: {
			pie: {
				show: true,
				innerRadius: 0.4,
				label: {
					
					show: true,
					radius: 3 / 4,
					formatter: function (label, series) {
						return "<div class=\"donut-label\" style=\"font-size:8pt;text-align:center;padding:2px;\" >" + label + "<br/>" + series.data[0][1] + "</div>";
					}
				}
			},
		},
		legend: { labelBoxBorderColor: "#ddd", backgroundColor: "none",show:false },
		grid: {
			hoverable: true
		},
		colors: ["#d9d9d9", "#5399D6", "#d7ea2b", "#348fe2", "#49b6d6", "#f59c1a", "#727cb6", "#ff5b57"],
	};

	options = $.extend({}, defaultOptions, options);

	$.plot($select, data, options);

}


//	bar chart
function BarChart($select, data, options) {

	var defaultOptions = {
		series: {
			bars: {
				show: true,
				align: 'center',
				barWidth: 0.5,
				lineWidth: 0,
				label: {
					show: true,
				},
				fillColor: {
					colors: [{
						opacity: 0.8
					}, {
						opacity: 0.8
					}]
				},
				numbers: {
					show : true
					}
			}
		},
		legend: {
			show: true,
		},
		grid: { hoverable: true, borderWidth: 0, labelMargin: 0, axisMargin: 0, minBorderMargin: 0 },
		colors: ["#d9d9d9", "#5399D6", "#d7ea2b", "#348fe2", "#49b6d6", "#f59c1a", "#727cb6", "#ff5b57"]
	};

	options = $.extend({}, defaultOptions, options);

	$.plot($select, data, options);

	if (options.isUseTooltip) {
		$($select).UseTooltip();
	}
}

var previousPoint = null, previousLabel = null;

$.fn.UseTooltip = function () {
	$(this).bind("plothover", function (event, pos, item) {
		if (item) {
			if ((previousLabel != item.series.label) || (previousPoint != item.dataIndex)) {
				previousPoint = item.dataIndex;
				previousLabel = item.series.label;
				$("#tooltip").remove();

				var x = item.datapoint[0];
				var y = item.datapoint[1];

				var color = item.series.color;

				//console.log(item.series.xaxis.ticks[x].label);                

				showTooltip(item.pageX,
				item.pageY,
				color,
				"<strong>" + item.series.label + "</strong><br>" + item.series.xaxis.ticks[x].label + " : <strong>" + y + "</strong>");
			}
		} else {
			$("#tooltip").remove();
			previousPoint = null;
		}
	});
};

function showTooltip(x, y, color, contents) {
	$('<div id="tooltip">' + contents + '</div>').css({
		position: 'absolute',
		display: 'none',
		top: y - 40,
		left: x - 120,
		border: '2px solid ' + color,
		padding: '3px',
		'font-size': '9px',
		'border-radius': '5px',
		'background-color': '#fff',
		'font-family': 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
		opacity: 0.9
	}).appendTo("body").fadeIn(200);
}



function bindBarChart(containerId, data) {

		var ds = [];
		var ticks = [];

		$.each(data.data, function (index) {

			ticks.push([index, this.label]);

			ds.push({
				label: this.label,
				data: [[index, this.data]]
			});

		});


		BarChart(containerId, ds, {
			xaxis: {
				ticks: ticks,
				tickLength: 0
			},
			yaxis: {
				ticks: [],
				tickLength: 0
			},
			legend: {
				show: false
			},
			isUseTooltip: true
		});

		$(containerId).UseTooltip();
}


function bindHorizontalBar($select, data) {

	var ds = new Array();
	var ticks = new Array();

	$.each(data.data, function (index) {

		ticks.push([index, this.label]);

		ds.push(
			[this.data,index]
		);

	});

	var dataSet = [{ data: ds }];

	var options = {
		series: {
			bars: {
				show: true
			}
		},
		
		bars: {
			align: "center",
			barWidth: 0.5,
			horizontal: true,
			fillColor: { colors: [{ opacity: 0.5 }, { opacity: 1 }] },
			lineWidth: 0,
			//colors: ["#d9d9d9", "#5399D6", "#d7ea2b", "#348fe2", "#49b6d6", "#f59c1a", "#727cb6", "#ff5b57"],
		},
		xaxis: {
			ticks: [],
		},
		yaxis: {
			axisLabel: "Precious Metals",
			axisLabelUseCanvas: true,
			axisLabelFontSizePixels: 12,
			axisLabelFontFamily: 'Verdana, Arial',
			axisLabelPadding: 3,
			tickColor: "#5E5E5E",
			ticks: ticks,
			color: "black",
			tickLength: 2
		},
		grid: { hoverable: true, borderWidth: 0, labelMargin: 0, axisMargin: 0, minBorderMargin: 0 }
	};

	var somePlot=$.plot($select, dataSet, options);



		// after initial plot draw, then loop the data, add the labels
		// I'm drawing these directly on the canvas, NO HTML DIVS!
		// code is un-necessarily verbose for demonstration purposes
		var ctx = somePlot.getCanvas().getContext("2d"); // get the context
		var allSeries = somePlot.getData();  // get your series data
		var xaxis = somePlot.getXAxes()[0]; // xAxis
		var yaxis = somePlot.getYAxes()[0]; // yAxis
		var offset = somePlot.getPlotOffset(); // plots offset
		ctx.font = "12px 'Segoe UI'"; // set a pretty label font
		ctx.fillStyle = "black";
		for (var i = 0; i < allSeries.length; i++) {
			var series = allSeries[i];
			var dataPoint = series.datapoints.points; // one point per series
			for (j = 0; j < dataPoint.length; j++) {
				var x = dataPoint[j];
				var y = dataPoint[j+1];
				var text = x == 0 || (y==0 && j>0) ? "" : x;
				var metrics = ctx.measureText(text);
				var xPos = xaxis.p2c(x) + offset.left - metrics.width; // place at end of bar
				var yPos = yaxis.p2c(y) + offset.top + 2;
				ctx.fillText(text, xPos, yPos);
			}
		}
}