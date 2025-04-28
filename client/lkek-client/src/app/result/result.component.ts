import { Component } from '@angular/core';
import { MatCardContent, MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-result',
  imports: [MatCardModule,MatCardContent],
  templateUrl: './result.component.html',
  styleUrl: './result.component.scss'
})
export class ResultComponent {
  imageUrl: string;

  constructor(private router: Router) {
    this.imageUrl = history.state.imageUrl || '';
    if (!this.imageUrl) {
      this.router.navigate(['/']);
    }
  }
}
