import { Component, Input } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-home-item',
  templateUrl: './home-item.component.html',
  styleUrl: './home-item.component.css'
})
export class HomeItemComponent {
  constructor(
    private readonly _router: Router,
  ) { }
  @Input() home: HomeCreatedModel | null = null;


  HomeInfo(homeId: string): void {
    if (homeId && this.home) {
      const navigationExtras: NavigationExtras = {
        state: {
          homeData: this.home
        }
      };
      this._router.navigate(['/homeOwners/individualHome', homeId])
        .catch(error => {
          console.error('Error de navegación:', error);
          this._router.navigateByUrl(`/homeOwners/individualHome/${homeId}`)
            .catch(err => console.error('Error en navegación alternativa:', err));
        });
    } else {
      console.error('homeId inválido:', homeId);
    }
  }
}
