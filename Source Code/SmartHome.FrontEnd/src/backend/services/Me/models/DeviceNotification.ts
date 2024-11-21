export interface DeviceNotification {
    id: string;
    homeDevice: string;
    event: string;
    read: boolean;
    date: Date;
    time: Date;
    detectedPerson: string | null;
}