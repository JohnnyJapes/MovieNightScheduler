import api from './api';

class GroupService {
    async createGroup(name, description) {
        return api.post("/Group/", {
            Name: name,
            Description: description,
        })
    }
    async getGroup(id) {
        return await api.get("/Group/" + id)
    }
}
export default new GroupService();