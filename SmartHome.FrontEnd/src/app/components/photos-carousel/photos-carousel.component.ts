import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-photos-carousel',
  templateUrl: './photos-carousel.component.html',
  styleUrl: './photos-carousel.component.css',
})
export class PhotosCarouselComponent {
  @Input({required: true}) photos!: Array<string>;
}
