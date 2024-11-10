import { User } from "./User";

export default interface DevicePaginatedResponse {
    data: Array<User>;
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}