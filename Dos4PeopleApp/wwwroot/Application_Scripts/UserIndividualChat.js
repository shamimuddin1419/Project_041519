$(document).ready(function () {
    loadInitialization();
    $('#ChatHistory').html(`<h1>dos4people</h1>`);

});
function loadInitialization() {
    GetIndividualChatList();    
};

var GetIndividualUnseenChatListByReceiverId = function () {
    $.get('/AdminIndividualChat/GetIndividualUnseenChatListByReceiverId', function (response) {
        var ChatHistory = '';
        if (response.status == true) {
            var itemRender
            var result = response.data;
            $('#lblNumberOfMessage').text(result.length);
            $('#lblNumberOfMessageForMobile').text(result.length);
            
            $.each(result, function (index, value) {
                if (value.isAdmin) {
                    itemRender = `<a href="/AdminIndividualChat">
                                            <div class="media">
                                                <div class="media-left align-self-center"><i class="ft-plus-square icon-bg-circle bg-cyan"></i></div>
                                                <div class="media-body">                                                   
                                                    <p class="notification-text font-small-3 text-muted"> ${value.messageBody}</p><small>
                                                        <time class="media-meta text-muted" datetime="2015-06-11T18:29:20+08:00"> ${value.createdDateString}</time>
                                                    </small>
                                                </div>
                                            </div>
                                        </a>`
                    ChatHistory = ChatHistory + itemRender;
                } else {
                    itemRender = `<a href="/UserIndividualChat">
                                            <div class="media">
                                                <div class="media-left align-self-center"><i class="ft-plus-square icon-bg-circle bg-cyan"></i></div>
                                                <div class="media-body">                                                   
                                                    <p class="notification-text font-small-3 text-muted"> ${value.messageBody}</p><small>
                                                        <time class="media-meta text-muted" datetime="2015-06-11T18:29:20+08:00"> ${value.createdDateString}</time>
                                                    </small>
                                                </div>
                                            </div>
                                        </a>`
                    ChatHistory = ChatHistory + itemRender;
                }

            });

            $('#ChatHistory').html(ChatHistory);
            $('#ChatHistoryForMobile').html(ChatHistory);

        } else {
            toastr.error(response.message);
        }

    });
}

function GetIndividualChatList() { 
        
    debugger;
    var ChatHistoryRender = '';
    var objVmChatting = {
        SenderID: ""
    };
    $.ajax({
        url: "/AdminIndividualChat/GetIndividualChatListForReceiver",
        data: JSON.stringify(objVmChatting),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            debugger;
            if (response.status == true) {               
                var itemRender
                //toastr.success(response.message);
                var result = response.data;
                $.each(result, function (index, value) {
                    $('#lblUserFullName').text(value.userName);
                    $('#lblUserMobile').text(value.mobile);
                    $('#lblUsergmail').text(value.email);
                    if (value.createdBy != value.dUser) {
                        itemRender =
                            `<div class="chat-message-right pb-4">
                                <div>
                                    <img src="/Content/Images/avatar1.png" class="rounded-circle mr-1" alt="Chris Wood" width="40" height="40">
                                    <div class="text-muted small text-nowrap mt-2">${value.createdDateString}</div>
                                </div>
                                <div class="flex-shrink-1 rounded py-2 px-3 mr-3" style="background-color: #EFEFEF">
                                    <div class="font-weight-bold">${value.senderName}</div>
                                    ${value.messageBody}
                                </div>
                            </div>`
                    }
                    else {
                        itemRender =
                            ` <div class="chat-message-left pb-4">
                                <div>
                                    <img src="/Content/Images/avatar5.png" class="rounded-circle mr-1" alt="Sharon Lessman" width="40" height="40">
                                    <div class="text-muted small text-nowrap mt-2">${value.createdDateString}</div>
                                </div>
                                <div class="flex-shrink-1 rounded py-2 px-3 ml-3" style="background-color: #EFEFEF">
                                    <div class="font-weight-bold">${value.senderName}</div>
                                    ${value.messageBody}
                                </div>
                            </div>`
                    }
                    ChatHistoryRender = ChatHistoryRender + itemRender;
                   
                });
                $('#ChatInvHistory').html(ChatHistoryRender);
                var messageBody = document.querySelector('#messageBody');
                messageBody.scrollTop = messageBody.scrollHeight - messageBody.clientHeight;
                UpdateIndividualUnseenChatStatus();
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("error!");
        }
    });
}

$('#btnSendIndividual').click(function () {
    if ($('#txtMessage').val().trim() == '') {
        toastr.warning("Empty message is not allowed")
    }
    else {
        var objChatting = {
            MessageBody: $('#txtMessage').val()
        };
        $.ajax({
            url: "/AdminIndividualChat/InsertIndividualChatForuser",
            data: JSON.stringify(objChatting),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    $('#txtMessage').val('')
                    GetIndividualChatList();
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (response) {
                toastr.error("error!");
            }
        });
    }
});
var UpdateIndividualUnseenChatStatus = function () {
    $.get('/AdminIndividualChat/UpdateIndividualUnseenChatStatus', function (response) {        
        if (response.status == true) {       
            GetIndividualUnseenChatListByReceiverId();
        } else {
            toastr.error(response.message);
        }

    });
}


