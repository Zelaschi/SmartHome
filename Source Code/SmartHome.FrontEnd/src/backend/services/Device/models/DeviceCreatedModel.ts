export default interface DeviceCreatedModel {
    id: string;
    type: string;
    name: string;
    modelNumber: string;
    description: string;
    photos: Array<string>;
    company: string;
}