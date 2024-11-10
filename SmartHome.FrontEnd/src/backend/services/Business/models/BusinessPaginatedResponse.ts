import { Business } from "./Business";

export default interface DevicePaginatedResponse {
    data: Array<Business>;
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}