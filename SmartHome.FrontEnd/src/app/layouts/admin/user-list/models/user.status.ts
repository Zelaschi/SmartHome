import { User } from "../../../../../backend/services/User/models/User";

export default interface UserStatus {
    moreUsers:boolean;
    loading?: true;
    users: Array<User>;
    error?: string;
}