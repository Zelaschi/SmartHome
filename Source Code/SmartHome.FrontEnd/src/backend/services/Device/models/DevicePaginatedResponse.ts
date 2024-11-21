import { Device } from "./Device";

export default interface DevicePaginatedResponse {
    data: Array<Device>;
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}