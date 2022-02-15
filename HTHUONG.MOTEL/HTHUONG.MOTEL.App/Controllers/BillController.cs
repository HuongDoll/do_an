using HTHUONG.MOTEL.App.BL.Bill;
using HTHUONG.MOTEL.Core.Constants;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.DTOs.AjaxResult;
using HTHUONG.MOTEL.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.Controllers
{
    public class BillController : BaseApiController
    {
        private readonly IBillService _billBL;

        public BillController(IBillService billBL)
        {
            _billBL = billBL;
        }

        [HttpGet]
        [Route("{billID}")]
        public async Task<IActionResult> GetBillAsync(string billID)
        {
            try
            {
                var userName = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                var bill = await _billBL.GetBillByIDAsync(billID);
                if (bill == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }


        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetBillsAsync([FromBody] GetListRequest getListRequest, long limit = 10, long offset = 0)
        {
            try
            {
                var userName = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                var data = await _billBL.GetBillsAsync(getListRequest, limit, offset);
                var total = await _billBL.CountBillAsync(getListRequest);
                return Ok(new
                {
                    Data = data.ToList(),
                    TotalRecords = total
                });
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertBillAsync([FromBody] Core.Entities.Bill bill)
        {
            try
            {
                var fullname = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(fullname))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                if (!ModelState.IsValid)
                {
                    var errorResponse = new ErrorResult
                    {
                        DevMsg = DevMessage.INVALID_MODEL,
                        UserMsg = UserMessage.INVALID_MODEL
                    };
                    errorResponse.ValidationFailures = CommonFunction.GetErrorMessages(ModelState);

                    return BadRequest(errorResponse);
                }



                var billID = await _billBL.InsertBillAsync(bill, fullname);
                if (string.IsNullOrEmpty(billID.ToString()))
                {
                    return BadRequest("INSERT_FAILED");
                }
                return StatusCode((int)HttpStatusCode.Created, billID);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult
                {
                    DevMsg = DevMessage.EXCEPTION,
                    UserMsg = UserMessage.CREATE_EXCEPTION
                });
            }
        }


        [HttpPut]
        [Route("{billID}")]
        public async Task<IActionResult> UpdateEmailByIDAsync(string billID, [FromBody] Core.Entities.Bill bill)
        {
            try
            {
                var fullname = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(fullname))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                if (!ModelState.IsValid)
                {
                    var errorResponse = new ErrorResult
                    {
                        DevMsg = DevMessage.INVALID_MODEL,
                        UserMsg = UserMessage.INVALID_MODEL
                    };
                    errorResponse.ValidationFailures = CommonFunction.GetErrorMessages(ModelState);
                    return BadRequest(errorResponse);
                }


                var oldBill = await _billBL.GetBillByIDAsync(billID);
                if (oldBill == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                


                var isUpdate = await _billBL.UpdateBillByIDAsync(bill, oldBill, fullname);

                return Ok(isUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult
                {
                    DevMsg = DevMessage.EXCEPTION,
                    UserMsg = UserMessage.CREATE_EXCEPTION
                });
            }
        }
    }
}
