$(document).ready(function () {
    loadInitialization();
    $('#ChatHistory').html(`<h1>dos4people</h1>`);

});
function loadInitialization() {
    GetIndividualChatUserList('');  
};

$("#UserSearchId").on("input", function () {   
    var SearchValue=$(this).val();
    GetIndividualChatUserList(SearchValue);
});

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
                                <div class="badge bg-success float-right">5</div>
                                <div class="d-flex align-items-start">
                                    <img src="https://bootdey.com/img/Content/avatar/avatar5.png" class="rounded-circle mr-1" alt="Vanessa Tucker" width="40" height="40">
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
function GetUserChattingHistory(userID, fullName,gmail,mobile) {   
    GetIndividualChatList(userID, fullName,gmail, mobile);
}
function GetIndividualChatList(ReceiverID, fullName, gmail, mobile) {
    $('#lblUserFullName').text(fullName);
    $('#lblUsergmail').text(gmail);    
    $('#lblUserMobile').text(mobile); 

    var ChatHistoryRender = '';
    var objVmChatting = {       
        ReceiverID: ReceiverID
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
                    if (value.createdBy == "7c019b92-997e-4892-9fc5-98d92bfc0651") {
                        itemRender =
                            `<div class="chat-message-right pb-4">
                                <div>
                                    <img src="https://bootdey.com/img/Content/avatar/avatar1.png" class="rounded-circle mr-1" alt="Chris Wood" width="40" height="40">
                                    <div class="text-muted small text-nowrap mt-2">2:33 am</div>
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
                                    <img src="https://bootdey.com/img/Content/avatar/avatar3.png" class="rounded-circle mr-1" alt="Sharon Lessman" width="40" height="40">
                                    <div class="text-muted small text-nowrap mt-2">2:34 am</div>
                                </div>
                                <div class="flex-shrink-1 rounded py-2 px-3 ml-3" style="background-color: #EFEFEF">
                                    <div class="font-weight-bold">${value.receiverName}</div>
                                    ${value.messageBody}
                                </div>
                            </div>`
                    }
                    ChatHistoryRender = ChatHistoryRender + itemRender;

                });
                $('#ChatHistory').html(ChatHistoryRender);
                var messageBody = document.querySelector('#messageBody');
                messageBody.scrollTop = messageBody.scrollHeight - messageBody.clientHeight;

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
    
    var objVmChatting = {       
        ReceiverID: "ce6f16cf-d418-4302-993f-bd6d11d76cb6",
        MessageBody: $('#txtMessage').val()
    };
    $.ajax({
        url: "/AdminIndividualChat/InsertIndividualChat",
        data: JSON.stringify(objVmChatting),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            debugger;
            if (response.status == true) {               
                GetIndividualChatList();
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("error!");
        }
    });

});