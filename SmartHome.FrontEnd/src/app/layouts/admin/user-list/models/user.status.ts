import { User } from "../../../../../backend/services/User/models/User";

export default interface UserStatus {
    loading?: true;
    users: Array<User>;
    error?: string;
}