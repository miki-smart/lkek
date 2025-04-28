import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [MatCardModule, MatButtonModule, MatInputModule, MatIconModule, ReactiveFormsModule],
})
export class HomeComponent {
  portrait: File | null = null;
  outfit: File | null = null;
  portraitPath: string | null = null;
  outfitPath: string | null = null;

  constructor(private http: HttpClient, private router: Router) {}

  onFileSelected(event: Event, type: 'portrait' | 'outfit') {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const formData = new FormData();
      formData.append(type, file);
      this.http.post(`/api/upload-${type}`, formData).subscribe({
        next: (response: any) => {
          if (type === 'portrait') {
            this.portrait = file;
            this.portraitPath = response.path;
          } else {
            this.outfit = file;
            this.outfitPath = response.path;
          }
        },
        error: (err) => alert('Upload failed: ' + err.message)
      });
    }
  }

  generateTryOn() {
    if (this.portraitPath && this.outfitPath) {
      this.http
        .post('/api/generate-try-on', {
          portrait: this.portraitPath,
          outfit: this.outfitPath
        })
        .subscribe({
          next: (response: any) => {
            this.router.navigate(['/result'], { state: { imageUrl: response.resultImageUrl } });
          },
          error: (err) => alert('Try-on failed: ' + err.message)
        });
    }
  }
}
