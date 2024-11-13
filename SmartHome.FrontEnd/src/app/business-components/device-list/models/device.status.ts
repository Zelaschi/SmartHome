import { Device } from "../../../../backend/services/Device/models/Device";

export default interface DeviceStatus {
    loading?: true;
    devices: Array<Device>;
    error?: string;
}