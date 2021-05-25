using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMXServiceInterfaceLib;
using System.Web.Routing;
using MMXDataService;
using MMXModelsLib;
using System.IO;

namespace MMXApplication.Controllers {
    public class MemberController : BaseController {

        private IMemberService memberService { get; set; }
        //public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);
            if (memberService == null) {
                memberService = new MemberService();
            }
        }


        [Authorize]
        public ActionResult AddUpdateMotorcycle(int motorcycleKey) {
            SetTabClasses(applicationTab);
            MotorcycleModel model = null;
            if (motorcycleKey < 1) {
                model = new MotorcycleModel();
                model.MemberKey = MemberKey;
            } else {
                model = memberService.GetMemberMotorcycle(motorcycleKey, MemberKey);
            }
            return BaseView("AddUpdateMotorcycle", model, "Add Update Motorcycle");
        }

        [Authorize]
        public ActionResult MotorcycleList() {
            SetTabClasses(applicationTab);
            return BaseView("MotorcycleList", memberService.GetMemberMotorcycles(MemberKey), "Motorcycle List");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUpdateMotorcycle(MotorcycleModel model) {
            SetTabClasses(applicationTab);
            if (ModelState.IsValid) {
                if (model.MotorcycleKey == 0) {
                    memberService.CreateMotorcycle(model);
                } else {
                    memberService.UpdateMotorcycle(model);
                }
                return MotorcycleList();
            } else {
                return AddUpdateMotorcycle(model.MotorcycleKey);
            }
        }


        [Authorize]
        //todo change to members get list from view
        public ActionResult Member() {
            SetTabClasses(applicationTab);
            MemberModel model = memberService.GetMember(MemberKey);
            if (model == null || model.MemberKey == 0) {
                model = new MemberModel() {
                    MemberKey = this.MemberKey,
                    Address = new AddressModel()
                };
            }            
            return BaseView("Member", model, "Member Info");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUpdateMember(MemberModel model) {
            if (ModelState.IsValid) {
                if (model.MemberKey < 1) {
                    memberService.CreateMember(model);
                } else {
                    memberService.UpdateMember(model);
                }
                return Member();
            } else {
                return BaseView("AddUpdateMember", model, "Add Update Member");
            }

        }

        [Authorize]
        public ActionResult AddUpdateMember(int memberKey) {
            SetTabClasses(memberTab);
            MemberModel model = null;
            if (memberKey < 1) {
                model = new MemberModel() {
                    Address = new AddressModel()
                };
            } else {
                model = memberService.GetMember(memberKey);
                if (model == null || model.MemberKey == 0) {
                    model = new MemberModel() {
                        Address = new AddressModel()
                    };
                }
            }
            return BaseView("AddUpdateMember", model, "Add Update Member");
        }      

        public ActionResult MemberList() {
            SetTabClasses(memberTab);
            // todo remove currentYear
            short currentYear = 2013; 
            List<MemberDisplayModel> models = memberService.GetMembers(currentYear);
            ViewData.Add("IsAdmin", IsAdminRole);
            return BaseView("MemberList", models, "Current Members");
        }

        public ActionResult MemberApplication() {
            SetTabClasses(applicationTab);
            SuperUserDashboardModel sModel = new SuperUserDashboardModel() {
                MemberKey = this.MemberKey
            };
            return BaseView("MemberApplication", sModel);
        }

        public FileResult MemberApplicationPDF() {         
           return File(Server.MapPath("~/App_Data/MMXData/mmx2014application.pdf"), "application/pdf");

        }

       
    }
}
