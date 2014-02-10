$(document).ready(function () {

    /*function getText(elem) { // only allow input[type=text]/textarea
        if(elem.tagName === "TEXTAREA" ||
           (elem.tagName === "INPUT" && elem.type === "text")) {
            return elem.value.substring(elem.selectionStart,
                                        elem.selectionEnd);
            // or return the return value of Tim Down's selection code here
        }
        return null;
    }*/

    $("#btnRecognize").click(function () {
        var sourceTextAreaValue = $("#sourceTextArea").val();
        if (sourceTextAreaValue.length > 3) {
            $.support.cors = true;
            $("#btnRecognize").attr("disabled", true);
            $.ajax({
                type: "POST",
                url: 'http://localhost:1885/LanguageRecognitionService.svc/Recognize/' + sourceTextAreaValue,
                dataType: "json",
                success: function (data, status) {
                    $("#btnRecognize").attr("disabled", false);
                    $("#sourceTextArea").tooltip({
                        content: data.RecognizeResult,
                        position: {
                            my: "center bottom-20",
                            at: "center top",
                            using: function (position, feedback) {
                                $(this).css(position);
                                $("<div>")
                                  .addClass("arrow")
                                  .addClass(feedback.vertical)
                                  .addClass(feedback.horizontal)
                                  .appendTo(this);
                            }
                        }
                    });
                    $("#sourceTextArea").val("Recognized: " + sourceTextAreaValue + "; see tooltip.");
                },

                error: function (response){ 
                    $("#btnRecognize").attr("disabled", false);
                    alert("Error: " + response.status + " " + response.statusText);
                }
            });
        } else {
            $("#sourceTextArea").tooltip({
                content: "Text length must be greater or equals 4.",
                position: {
                    my: "center bottom-20",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                          .addClass("arrow")
                          .addClass(feedback.vertical)
                          .addClass(feedback.horizontal)
                          .appendTo(this);
                    }
                }
            });
        }
    });
});