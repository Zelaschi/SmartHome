import { DeviceNotification } from "../../../../../backend/services/Me/models/DeviceNotification";

export default interface NotificationStatus {
    loading?: true;
    notifications: Array<DeviceNotification>;
    error?: string;
}