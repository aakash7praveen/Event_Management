import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@Component({
  standalone: true,
  selector: 'app-error-dialog-box',
  imports: [MatDialogModule, MatIconModule],
  templateUrl: './error-dialog-box.component.html',
  styleUrl: './error-dialog-box.component.scss'
})
export class ErrorDialogBoxComponent {
  constructor(
    public dialogRef: MatDialogRef<ErrorDialogBoxComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  close(): void {
    this.dialogRef.close();
  }
}
