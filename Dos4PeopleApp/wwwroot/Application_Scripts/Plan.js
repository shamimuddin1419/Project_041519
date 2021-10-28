$(document).ready(function () {
    planListRender();
});
var planListRender = function () {
    var packageListRender = '';
    $.get("/Package/GetPublishedPackageList", function (response) {
        debugger;
        if (response.status == true) {
            //toastr.success(response.message);
            var result = response.data;
            $.each(result, function (index, value) {
                var itemRender =
                    `<div class="col-lg-4 col-md-4" data-aos="fade-up" data-aos-delay="500">
                            <div class="pricing_plan_1 bg-white amount pr-0 pl-0">
                                <div class="pricing_header mb-50">
                                    <h3 class="title_text mb-30 text_lightblue">${value.packageName}</h3>
                                    <div class="price_text monthly_price">
                                        <sup>$</sup>
                                        <strong>${value.packageValue}</strong>
                                    </div>
                                </div>
                                <div class="info_list ul_li_block">
                                    <ul class="clearfix">
                                        <li>Plan Duration  ${value.packageDurationDays}</li>
                                        <li>Just ${value.dailyTaskCount} Task  ${value.dailyTaskCount}</li>
                                        <li>Action worth  <strong>$</strong>${value.perClickValue}</li>
                                        <li>Daily earn  <strong>$</strong>${value.dailyValue}</li>
                                        <li>Weekly earn  <strong>$</strong>${value.weeklyValue}</li>
                                        <li>Monthly earn  <strong>$</strong>${value.monthlyValue}</li>
                                        <li>Yearly earn  <strong>$</strong>${value.yearlyValue}</li>
                                        <li>Referral Earn ${value.maxLevel} Label AM ${value.referralEarn}</li>
                                        <li>Work Comission ${value.maxLevel} Label AM ${value.workCommission}</li>
                                        <li> Potential Referral Earn  ${value.potentialReferralEarn}</li>	
                                        <li>Potential Referral Work Earn  ${value.potentialReferralWorkEarn}</li>	
                                        <li>Target Potential Yearly Income	${value.targetPotentialYearlyIncome}</li>
                                        ${value.tcbOnMainInvestPer === 0 ? '' : '<li>TCB On the Main Investment	'+value.tcbOnMainInvestPer+'</li>'}
                                        <li>Potential Yearly Min Income	 ${value.potentialYearlyIncome}</li>
                                        <li>${value.remarks}</li>
                                        
                                    </ul>
                                </div>
                                <a href="/Buyplan/Index?packageId=${value.packageId}" class="btn btn_border border_blue">Buy Now</a>
                            </div>
                        </div>`
                packageListRender = packageListRender + itemRender;
            });
            $('#PriceLoadingId').html(packageListRender);


        } else {
            toastr.error(response.message);
        }
    })
};