import { DeviceNotification } from "../../../../../backend/services/Me/models/DeviceNotification";

export default interface NotificationStatus {
    loading?: boolean;
    notifications: Array<DeviceNotification>;
    error?: string;
}