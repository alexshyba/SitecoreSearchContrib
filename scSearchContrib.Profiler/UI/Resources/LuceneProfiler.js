var $j = jQuery.noConflict();
var profiler = profiler || {};

profiler.load = (function (data) {
    $j("#traceTemplate").tmpl(data).appendTo("#traceTable");
});
   
profiler.toggle = (function(container) {
    console.log("click");

    if ($j(container).is(":visible"))
        $j(container).hide();
    else
        $j(container).show();

    return false;
});