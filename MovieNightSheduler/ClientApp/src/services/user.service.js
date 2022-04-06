import api from './api';

class UserService {
    async getUserInfo(userId) {
        return await api.get("/User/" + userId);
    }
}
export default new UserService();