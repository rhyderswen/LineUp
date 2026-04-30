[**lineup-client**](../../../index.md)

---

[lineup-client](../../../modules.md) / [utils/time](../index.md) / formatTime24Hour

# Function: formatTime24Hour()

> **formatTime24Hour**(`time`): `string`

Defined in: [utils/time.ts:98](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/time.ts#L98)

Formats a [Time](../../../types/type-aliases/Time.md) object into 24H format without AM/PM.

## Parameters

### time

[`Time`](../../../types/type-aliases/Time.md)

## Returns

`string`

The formatted time string.

## Example

```ts
formatTime({ hour: 17, minute: 15 });
// Returns "17:15"
```
