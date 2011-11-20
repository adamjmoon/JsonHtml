$.fn.clearSelect = function() {
    return this.each(function() {
        if (this.tagName == 'SELECT')
            this.options.length = 0;
    });
 } 
 
$.fn.fillSelect = function(data) {
    return this.clearSelect().each(function() {
        if (this.tagName == 'SELECT') {
            var dropdownList = this;
            $.each(data, function(index, optionData) {

              var option = new Option();
              option.text = optionData.Text;
              option.value = optionData.Value;
              option.selected = optionData.Selected;


              if(option.selected){
                   $(option).attr("selected","selected");
              }
                
                if ($.browser.msie) {
                    dropdownList.add(option);
                }
                else {
                    dropdownList.add(option, null);
                }
            });
        }
    });
 }