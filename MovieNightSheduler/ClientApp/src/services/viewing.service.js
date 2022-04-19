import { date } from 'yup';
import api from './api';

class ViewingService {
    async createViewing(title, description, date, groupId, location) {
        return await api.post("/Viewing/", {
            Title: title,
            Description: description,
            Date: date,
            group_id: groupId,
            Location: location
        })
    }
    async getViewingByGroupID(id) {
        return await api.get({
            url: "/Viewing/",
            data: {
                groupId: id
            }
        })

    }
}
export default new ViewingService();