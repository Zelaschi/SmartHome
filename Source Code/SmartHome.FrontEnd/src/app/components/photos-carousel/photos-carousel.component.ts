import { Component, Input } from '@angular/core';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-photos-carousel',
  templateUrl: './photos-carousel.component.html',
  standalone: true,
  imports: [NgbCarouselModule, CommonModule],
  styleUrl: './photos-carousel.component.css',
})
export class PhotosCarouselComponent {
  @Input({required: true}) photos!: Array<string>;
}
