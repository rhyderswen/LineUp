[**lineup-client**](../../../index.md)

---

[lineup-client](../../../modules.md) / [utils/time](../index.md) / formatTime

# Function: formatTime()

> **formatTime**(`time`): `string`

Defined in: [utils/time.ts:27](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/time.ts#L27)

Formats a [Time](../../../types/type-aliases/Time.md) object into 12H format with AM/PM.

## Parameters

### time

[`Time`](../../../types/type-aliases/Time.md)

## Returns

`string`

The formatted time string.

## Example

```ts
formatTime({ hour: 9, minute: 15 });
// Returns "09:15 AM"
```
