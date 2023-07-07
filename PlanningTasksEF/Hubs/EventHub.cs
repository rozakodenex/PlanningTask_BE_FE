using Microsoft.AspNetCore.SignalR;

namespace PlanningTasksEF.Hubs
{
    public class EventHub:Hub
    {
        public async Task EventUpdate(string eventName)
        {
            await Clients.All.SendAsync("RefreshGrid", eventName);
        }
    }
}
