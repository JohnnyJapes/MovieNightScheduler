import api from './api';

class GroupService {
    async createGroup(name, description) {
        return api.post("/Group/", {
            Name: name,
            Description: description,
        })
    }
    async getGroup(id) {
        return api.get("/Group/" + id)
    }
}
export default new GroupService();