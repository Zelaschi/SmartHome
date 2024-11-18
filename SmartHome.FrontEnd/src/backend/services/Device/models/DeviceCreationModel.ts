export default interface DeviceCreationModel {
    name: string,
    modelNumber: string,
    description: string,
    photos: Array<string>,
    type: string,
    personDetection: boolean | null,
    movementDetection: boolean | null,
    indoor: boolean | null,
    outdoor: boolean | null
}