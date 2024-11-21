import { Device } from "../../../../backend/services/Device/models/Device";

export default interface DeviceStatus {
    moreDevices: boolean;
    loading?: true;
    devices: Array<Device>;
    error?: string;
}