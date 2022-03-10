$(document).ready(function () {
    loadInitialization();
    $('#ChatHistory').html(`<h1>dos4people</h1>`);

});
function loadInitialization() {
    $('#idUnSeenMessagePart').hide();
    $('#idChattingPart').hide();
    GetIndividualChatUserList('');  
    GetNumberofUnseenChat();   
    GetIndividualUnseenChatListByReceiverId();
};

var GetIndividualUnseenChatListByReceiverId = function () {
    $.get('/AdminIndividualChat/GetIndividualUnseenChatListByReceiverId', function (response) {
        var ChatHistory = '';
        if (response.status == true) {
            var itemRender
            var result = response.data;
            $('#lblNumberOfMessage').text(result.length);
            $('#lblSubNumberOfMessage').text(result.length);
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

        } else {
            toastr.error(response.message);
        }

    });
}
var GetNumberofUnseenChat = function () {
    $.get('/AdminIndividualChat/IndividualUnseenChatListForAdmin', function (response) {        
        if (response.status == true) {
            debugger;
            var dataSet = response.data;
            if (dataSet.length > 0) {
                $('#idUnSeenMessagePart').show();
                var table = $('#ListTableId').DataTable();
                table.destroy();
                $('#ListTableId').DataTable({
                    data: dataSet,
                    "responsive": true,
                    //"processing": true,
                    //"serverSide": true,
                    "columns":
                        [
                            {
                                "data": null,
                                'width': '5%',
                                "className": "center",
                                render: function (data, type, row) {
                                    return `<button type="button" onclick = "ChattingHistoryfromGrdButton(' ${data.senderID} ',' ${data.senderName} ',' ${data.email} ',' ${data.mobile} ')" class="btn info"><i class="fa fa-pencil"></i></button>`
                                }

                            },

                            { "data": "senderName", "autoWidth": true },
                            { "data": "numberOfUnseenMessage", "autoWidth": true }
                        ],
                    "order": [1, "asc"],
                    "processing": "true",
                    "language": {
                        "processing": "processing... please wait"
                    },
                });
            } else {
                $('#idUnSeenMessagePart').hide();
                $('#idChattingPart').show();
            }
        } else {
            toastr.error(response.message);
        }
    });
}

function ChattingHistoryfromGrdButton(userID, fullName, gmail, mobile) {    
    GetIndividualChatList(userID, fullName, gmail, mobile, 1);
    $('#idUnSeenMessagePart').hide();
    $('#idChattingPart').show();
}

$("#UserSearchId").on("input", delay(function (e) {
    var SearchValue = $(this).val();
    GetIndividualChatUserList(SearchValue);
}));

function delay(fn, ms) {
    let timer = 0
    return function (...args) {
        clearTimeout(timer)
        timer = setTimeout(fn.bind(this, ...args), ms || 0)
    }
}

function GetIndividualChatUserList(searchValue) {
    var UserListRender = '';
    var objVmChatting = {
        searchValue: searchValue        
    };
    $.ajax({
        url: "/AdminIndividualChat/GetIndividualChatUserList",
        data: JSON.stringify(objVmChatting),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            debugger;
            if (response.status == true) {
                var itemRender              
                var result = response.data;
                $.each(result, function (index, value) {                
                        itemRender =
                            `<a class="list-group-item list-group-item-action border-0" onclick = "GetUserChattingHistory('${value.userId}','${value.fullName}','${value.email}','${value.mobile}')">                              
                                <div class="d-flex align-items-start">
                                    <img src="/Content/Images/avatar5.png" class="rounded-circle mr-1" alt="Vanessa Tucker" width="40" height="40">
                                    <div class="flex-grow-1">
                                          ${value.fullName}
                                        <div class="small">${value.email}</div>
                                        <div class="small">${value.mobile}</div>
                                    </div>
                                </div>
                            </a>`                    
                    UserListRender = UserListRender + itemRender;

                });

                $('#UserListBindId').html(UserListRender);                 

            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("error!");
        }
    });
}
function GetUserChattingHistory(userID, fullName, gmail, mobile) {
    debugger;
    GetIndividualChatList(userID, fullName,gmail, mobile,1);
}
function GetIndividualChatList(SenderID, fullName, gmail, mobile, callId) {
    if (callId == 1) {
        $('#lblUserFullName').text(fullName.trim());
        $('#lblUsergmail').text(gmail.trim());
        $('#lblUserMobile').text(mobile.trim());
        $('#hidSenderID').val(SenderID.trim());
    }
    debugger;
    var ChatHistoryRender = '';
    var objVmChatting = {       
        SenderID: SenderID.trim()
    };
    $.ajax({
        url: "/AdminIndividualChat/GetIndividualChatList",
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
                    if (value.createdBy != $('#hidSenderID').val()) {
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
            ReceiverID: $('#hidSenderID').val().trim(), // This is ReceiverID
            MessageBody: $('#txtMessage').val()
        };
        $.ajax({
            url: "/AdminIndividualChat/InsertIndividualChat",
            data: JSON.stringify(objChatting),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                debugger;
                if (response.status == true) {
                    $('#txtMessage').val('')
                    GetIndividualChatList($('#hidSenderID').val(), "", "", "", 2);
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
    var objChatting = {
        SenderID: $('#hidSenderID').val().trim()      
    };
    $.ajax({
        url: "/AdminIndividualChat/UpdateIndividualUnseenChatStatus",
        data: JSON.stringify(objChatting),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        success: function (response) {       
            GetIndividualUnseenChatListByReceiverId();
        },
        error: function (response) {
            toastr.error("error!");
        }
    });
}

$('#btnUnseenMessage').click(function () {
    $('#idUnSeenMessagePart').show();
    $('#idChattingPart').hide();   
    GetNumberofUnseenChat();
});
$('#btnShowChattingOption').click(function () {
    $('#idUnSeenMessagePart').hide();
    $('#idChattingPart').show();
});

