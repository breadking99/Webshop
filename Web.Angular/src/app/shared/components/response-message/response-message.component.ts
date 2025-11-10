import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Response } from '../../responses/response';
import { ResponseStatus } from '../../enums/response-status';

@Component({
  selector: 'app-response-message',
  standalone: false,
  templateUrl: './response-message.component.html',
  styleUrls: ['./response-message.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ResponseMessageComponent {
  @Input() response: Response<unknown> | null = null;

  get isVisible(): boolean {
    return !!this.response?.message && this.response.statusCode !== ResponseStatus.Default;
  }

  get cssClass(): string {
    if (!this.response) {
      return 'alert alert-secondary';
    }

    const status = this.response.statusCode as ResponseStatus;

    switch (status) {
      case ResponseStatus.Ok:
        return 'alert alert-success';
      case ResponseStatus.BadRequest:
      case ResponseStatus.Forbid:
      case ResponseStatus.Unauthorized:
      case ResponseStatus.InternalServerError:
        return 'alert alert-danger';
      case ResponseStatus.NotFound:
      case ResponseStatus.TimeOut:
        return 'alert alert-warning';
      case ResponseStatus.Loading:
        return 'alert alert-info';
      default:
        return 'alert alert-secondary';
    }
  }
}
