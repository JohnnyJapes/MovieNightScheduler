import api from './api';

class GroupService {
    async createGroup(name, description) {
        return api.post("/Group/", {
            Name: name,
            Description: description,
        })
    }
}
export default new GroupService();