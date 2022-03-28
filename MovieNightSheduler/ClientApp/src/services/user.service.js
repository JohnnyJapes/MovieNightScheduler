import api from './api';

class UserService {
    getUserInfo(userId) {
        return api.get("/User/" + userId);
    }
}
export default new UserService();