using DXWebApplication1.Model;
using System.Collections.Generic;

namespace DXWebApplication1.Model {
    public class SchedulerDataSource {
        List<long> selectedResourceIds;

        public SchedulerDataSource(List<long> selectedResourceIds) {
            this.selectedResourceIds = selectedResourceIds;
        }
        public List<SchedulerAppointment> Appointments {
            get { return AppointmentDataSourceHelper.SelectMethodHandler(); }
        }

        public List<SchedulerResource> Resources {
            get { return ResourceDataSourceHelper.GetItems(selectedResourceIds); }
        }

        public void InsertSchedulerAppointments(List<SchedulerAppointment> appointments) {
            appointments.ForEach(a => AppointmentDataSourceHelper.InsertMethodHandler(a));
        }
        public void UpdateSchedulerAppointments(List<SchedulerAppointment> appointments) {
            appointments.ForEach(a => AppointmentDataSourceHelper.UpdateMethodHandler(a));
        }
        public void RemoveSchedulerAppointments(List<SchedulerAppointment> appointments) {
            appointments.ForEach(a => AppointmentDataSourceHelper.DeleteMethodHandler(a));
        }
    }
}