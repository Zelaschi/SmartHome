export interface SecurityCamera {
    id: string;
    name: string;
    type: string;
    modelNumber: string;
    description: string;
    photos: string[];
    inDoor: boolean;
    outDoor: boolean;
    movementDetection: boolean;
    personDetection: boolean;
}