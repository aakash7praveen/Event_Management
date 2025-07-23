import { Component,Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog'
import { MatIconModule } from '@angular/material/icon';


@Component({
  standalone: true,
  selector: 'app-success-dialog-box',
  imports: [ MatDialogModule, MatIconModule ],
  templateUrl: './success-dialog-box.component.html',
  styleUrl: './success-dialog-box.component.scss'
})

export class SuccessDialogBoxComponent {
  constructor(
    public dialogRef: MatDialogRef<SuccessDialogBoxComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  close(): void {
    this.dialogRef.close();
  }
}
