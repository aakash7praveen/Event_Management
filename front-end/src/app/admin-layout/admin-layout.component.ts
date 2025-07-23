import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterModule, MatButtonModule],
  templateUrl: './admin-layout.component.html'
})
export class AdminLayoutComponent {
  ngOnInit() {
    console.log('AdminLayoutComponent loaded');
  }
}
